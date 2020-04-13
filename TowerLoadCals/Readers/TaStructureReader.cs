using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TowerLoadCals.Readers
{
    public class TaStructureReader
    {
        public static TaStructure Read(string path)
        {
            TaStructure taStructure = new TaStructure();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //解析XML节点
            XmlNode xmlNode = doc.GetElementsByTagName("XML")[0];
            if (xmlNode == null)
                return null;

            taStructure.Name = xmlNode.Attributes["Name"].Value.ToString();
            taStructure.CircuitNum  = Convert.ToInt16(xmlNode.Attributes["Circuit"].Value.ToString());
            taStructure.Type = Convert.ToInt16(xmlNode.Attributes["Type"].Value.ToString());
            taStructure.Category = xmlNode.Attributes["Category"].Value.ToString();
            taStructure.AppearanceType = xmlNode.Attributes["AppearanceType"].Value.ToString();

            //解析CircuitSet节点
            XmlNode circuitZBaseNode = doc.GetElementsByTagName("CircuitZBase")[0];
            if (circuitZBaseNode == null)
                return null;
            taStructure.ZBaseCircuitId = Convert.ToInt16(circuitZBaseNode.Attributes["Circuit"].Value.ToString());
            taStructure.ZBasePhaseId = Convert.ToInt16(circuitZBaseNode.Attributes["PhaseId"].Value.ToString());

            //解析CircuitSet节点
            XmlNode circuitSetNode = doc.GetElementsByTagName("CircuitSet")[0];
            if (circuitSetNode == null)
                return null;

            taStructure.CircuitSet = new List<Circuit>();
            foreach (XmlNode csNode in circuitSetNode.ChildNodes)
            {
                Circuit cs = new Circuit();
                cs.Name = csNode.Attributes["Name"].Value.ToString();
                cs.Id = Convert.ToInt16(csNode.Attributes["Id"].Value.ToString());

                if(csNode.Attributes["Current"] == null)
                {
                    cs.IsCurrentExist = false;
                }
                else
                {
                    cs.IsCurrentExist = true;
                    cs.Current = csNode.Attributes["Current"].Value.ToString();
                }

                if (csNode.Attributes["Voltage"] == null)
                {
                    cs.IsVoltageExist = false;
                }
                else
                {
                    cs.IsVoltageExist = true;
                    cs.Voltage = Convert.ToInt16(csNode.Attributes["Voltage"].Value.ToString());
                }

                cs.PhaseWires = new List<PhaseWire>();

                foreach(XmlNode wireNode in csNode.ChildNodes)
                {
                    PhaseWire wire = new PhaseWire();
                    wire.Name = wireNode.Attributes["PhaseName"].Value.ToString();
                    wire.Id = Convert.ToInt16(wireNode.Attributes["PhaseId"].Value.ToString());
                    wire.Postions = new List<CircuitPostion>();

                    cs.PhaseWires.Add(wire);
                }

                taStructure.CircuitSet.Add(cs);
            }

            //解析CircuitSet节点

            XmlNodeList CircuitPosNodeList = doc.GetElementsByTagName("CircuitPos");

            foreach(XmlNode csPosNode in CircuitPosNodeList)
            {
                int csId = Convert.ToInt16(csPosNode.Attributes["Circuit"].Value.ToString());
                int phaseId = Convert.ToInt16(csPosNode.Attributes["PhaseId"].Value.ToString());

                XmlNode posNode = csPosNode.FirstChild;
                
                if(posNode != null)
                {
                    CircuitPostion pos = new CircuitPostion
                    {
                        FunctionType = csPosNode.Attributes["FunctionType"].Value.ToString(),
                        Px = Convert.ToInt16(posNode.Attributes["Px"].Value.ToString()),
                        Py = Convert.ToInt16(posNode.Attributes["Py"].Value.ToString()),
                        Pz = Convert.ToInt16(posNode.Attributes["Pz"].Value.ToString())
                    };

                    Circuit circuit = taStructure.CircuitSet.Where(item => item.Id == csId).ToList().First();
                    if (circuit == null)
                        continue;

                    PhaseWire pw = circuit.PhaseWires.Where(item => item.Id == phaseId).ToList().First();
                    if (pw != null)
                    {
                        pw.Postions.Add(pos);
                    }
                }

            }

            return taStructure;
        }


        public static void Save(string path, TaStructure taStructure)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode decNode =  doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(decNode);

            XmlNode xmlNode = doc.CreateElement("XML");
            doc.AppendChild(xmlNode);

            XmlAttribute nameAttribute = doc.CreateAttribute("Name");
            nameAttribute.Value = taStructure.Name;
            xmlNode.Attributes.Append(nameAttribute);

            XmlAttribute circuitNumAttribute = doc.CreateAttribute("Circuit");
            circuitNumAttribute.Value = taStructure.CircuitNum.ToString();
            xmlNode.Attributes.Append(circuitNumAttribute);

            XmlAttribute typeAttribute = doc.CreateAttribute("Type");
            typeAttribute.Value = taStructure.Type.ToString();
            xmlNode.Attributes.Append(typeAttribute);

            XmlAttribute categoryeTypeAttribute = doc.CreateAttribute("Category");
            categoryeTypeAttribute.Value = taStructure.Category;
            xmlNode.Attributes.Append(categoryeTypeAttribute);

            XmlAttribute appearanceTypeAttribute = doc.CreateAttribute("AppearanceType");
            appearanceTypeAttribute.Value = taStructure.AppearanceType;
            xmlNode.Attributes.Append(appearanceTypeAttribute);

            XmlNode circuitSetNode = doc.CreateElement("CircuitSet");
            xmlNode.AppendChild(circuitSetNode);

            XmlNode circuitPosSetNode = doc.CreateElement("CircuitPosSet");
            xmlNode.AppendChild(circuitPosSetNode);

            XmlNode circuitZBasNode = doc.CreateElement("CircuitZBase");
            circuitPosSetNode.AppendChild(circuitZBasNode);

            XmlAttribute zBaseCircuitIdAttribute = doc.CreateAttribute("Circuit");
            zBaseCircuitIdAttribute.Value = taStructure.ZBaseCircuitId.ToString();
            circuitZBasNode.Attributes.Append(zBaseCircuitIdAttribute);

            XmlAttribute zBasePhaseIdAttribute = doc.CreateAttribute("PhaseId");
            zBasePhaseIdAttribute.Value = taStructure.ZBasePhaseId.ToString();
            circuitZBasNode.Attributes.Append(zBasePhaseIdAttribute);

            foreach(var csItem in taStructure.CircuitSet)
            {
                XmlNode circuitNode = doc.CreateElement("Circuit");
                circuitSetNode.AppendChild(circuitNode);

                XmlAttribute csNameAttribute = doc.CreateAttribute("Name");
                csNameAttribute.Value = csItem.Name;
                circuitNode.Attributes.Append(csNameAttribute);

                XmlAttribute csIdAttribute = doc.CreateAttribute("Id");
                csIdAttribute.Value = csItem.Id.ToString();
                circuitNode.Attributes.Append(csIdAttribute);

                if(csItem.IsCurrentExist)
                {
                    XmlAttribute csCurrenttAttribute = doc.CreateAttribute("Current");
                    csCurrenttAttribute.Value = csItem.Current;
                    circuitNode.Attributes.Append(csCurrenttAttribute);
                }

                if (csItem.IsVoltageExist)
                {
                    XmlAttribute csVoltageAttribute = doc.CreateAttribute("Voltage");
                    csVoltageAttribute.Value = csItem.Voltage.ToString();
                    circuitNode.Attributes.Append(csVoltageAttribute);
                }

                foreach (var wireItem in csItem.PhaseWires)
                {
                    XmlNode wireNode = doc.CreateElement("PhaseWire");
                    circuitNode.AppendChild(wireNode);

                    XmlAttribute phaseNameAttribute = doc.CreateAttribute("PhaseName");
                    phaseNameAttribute.Value = wireItem.Name;
                    wireNode.Attributes.Append(phaseNameAttribute);

                    XmlAttribute phaseIdAttribute = doc.CreateAttribute("PhaseId");
                    phaseIdAttribute.Value = wireItem.Id.ToString();
                    wireNode.Attributes.Append(phaseIdAttribute);

                    foreach(var pos in wireItem.Postions)
                    {
                        XmlNode posNode = doc.CreateElement("CircuitPos");
                        circuitPosSetNode.AppendChild(posNode);

                        XmlAttribute circuitAttribute = doc.CreateAttribute("Circuit");
                        circuitAttribute.Value = csItem.Id.ToString();
                        posNode.Attributes.Append(circuitAttribute);

                        XmlAttribute phaseAttribute = doc.CreateAttribute("PhaseId");
                        phaseAttribute.Value = wireItem.Id.ToString();
                        posNode.Attributes.Append(phaseAttribute);

                        XmlAttribute functionTypeAttribute = doc.CreateAttribute("FunctionType");
                        functionTypeAttribute.Value = pos.FunctionType;
                        posNode.Attributes.Append(functionTypeAttribute);

                        XmlNode positionNode = doc.CreateElement("Position");
                        posNode.AppendChild(positionNode);

                        XmlAttribute pzAttribute = doc.CreateAttribute("Pz");
                        pzAttribute.Value = pos.Pz.ToString();
                        positionNode.Attributes.Append(pzAttribute);

                        XmlAttribute pyAttribute = doc.CreateAttribute("Py");
                        pyAttribute.Value = pos.Py.ToString();
                        positionNode.Attributes.Append(pyAttribute);

                        XmlAttribute pxAttribute = doc.CreateAttribute("Px");
                        pxAttribute.Value = pos.Px.ToString();
                        positionNode.Attributes.Append(pxAttribute);
                    }
                }
            }

            doc.Save(path);
        }
    }
}