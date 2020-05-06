using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using TowerLoadCals.Mode;
using TowerLoadCals.Common;

namespace TowerLoadCals.DAL
{

    //public abstract class WireReader
    //{
    //    public abstract List<WireLib> Read(string path);
    //    public abstract List<WireLib> Read(string dir, string tableName);
    //}

    //public class WireXmlReader : WireReader
    //{
    //    public override List<WireLib> Read(string path)
    //    {
    //        XmlDocument doc = new XmlDocument();
    //        doc.Load(path);

    //        XmlNode rootNode = doc.FirstChild;
    //        if (rootNode == null)
    //            return new List<WireLib>();

    //        XmlNode libNameTitleNode = rootNode.FirstChild;
    //        if (libNameTitleNode == null)
    //            return new List<WireLib>();

    //        List<WireLib> list = new List<WireLib>();

    //        foreach (XmlNode libNode in libNameTitleNode.ChildNodes)
    //        {
    //            List<WireType> typeList = new List<WireType>();

    //            foreach(XmlNode typeNode in libNode.ChildNodes)
    //            {
    //                List<Wire> wireList = new List<Wire>();

    //                foreach(XmlNode wireNode  in typeNode.ChildNodes )
    //                {
    //                    List<WireSpec> specList = new List<WireSpec>();

    //                    foreach (XmlNode specNode in wireNode.ChildNodes)
    //                    {
    //                        WireSpec wireSpec = new WireSpec
    //                        {
    //                            ModelSpecification = specNode.Attributes["SModelSpecification"].Value.ToString(),
    //                            SectionArea = specNode.Attributes["SSectionAreaTotal"].Value.ToString(),
    //                            ExternalDiameter = specNode.Attributes["SExternalDiameter"].Value.ToString(),
    //                            UnitLengthMass = specNode.Attributes["SUnitLengthMass"].Value.ToString(),
    //                            DCResistor = specNode.Attributes["SDCResistor"].Value.ToString(),
    //                            RatedBreakingForce = specNode.Attributes["SRatedBreakingForce"].Value.ToString(),
    //                            ModulusElasticity = specNode.Attributes["SModulusElasticity"].Value.ToString(),
    //                            LineCoefficient = specNode.Attributes["SLineCoefficient"].Value.ToString(),
    //                        };

    //                        specList.Add(wireSpec);
    //                    }

    //                    Wire wire = new Wire
    //                    {
    //                        Name = wireNode.Attributes["SName"].Value.ToString(),
    //                        Specs = specList
    //                    };
    //                    wireList.Add(wire);

    //                }

    //                WireType wireType = new WireType
    //                {
    //                    Type = typeNode.Attributes["SName"].Value.ToString(),
    //                    Wire = wireList
    //                };

    //                typeList.Add(wireType);
    //            }

    //            WireLib wireLib = new WireLib
    //            {
    //                Lib = libNode.Attributes["SName"].Value.ToString(),
    //                Types = typeList
    //            };

    //            list.Add(wireLib);
    //        }

    //        return list;
    //    }

    //    public override List<WireLib> Read(string dir, string tableName)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class WireDbfReader : WireReader
    //{
    //    public override List<WireLib> Read(string path)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override List<WireLib> Read(string dir, string tableName)
    //    {
    //        DataTable dataTable = DbfUtils.ReadDbf(dir, tableName);

    //        List<WireSpec> specs = new List<WireSpec>();

    //        foreach (DataRow row in dataTable.Rows)
    //        {
    //            specs.Add(new WireSpec
    //            {
    //                ModelSpecification = row["电线型号"].ToString(),
    //                SectionArea = row["截面MM2"].ToString(),
    //                ExternalDiameter = row["外径MM"].ToString(),
    //                UnitLengthMass = row["重量KG每KM"].ToString(),
    //                RatedBreakingForce = row["断拉力N"].ToString(),
    //                ModulusElasticity = row["弹性系数N"].ToString(),
    //                LineCoefficient = row["线膨胀系数"].ToString(),
    //            });
    //        }

    //        Wire wire = new Wire
    //        {
    //            Name = "DBF",
    //            Specs = specs
    //        };

    //        List<Wire> wires = new List<Wire>();
    //        wires.Add(wire);

    //        WireType wireType = new WireType
    //        {
    //            Type = "DBF",
    //            Wire = wires
    //        };

    //        List<WireType> wireTypes = new List<WireType>();
    //        wireTypes.Add(wireType);

    //        WireLib wireLib = new WireLib
    //        {
    //            Lib = "DBF",
    //            Types = wireTypes
    //        };

    //        List<WireLib> wireLibs = new List<WireLib>();
    //        wireLibs.Add(wireLib);

    //        return wireLibs;
    //    }
    //}


    //public class WireReaderFactory
    //{
    //    public static WireReader CreateReader(string type)
    //    {
    //        WireReader wireReader = null;

    //        if(type.Equals("XML"))
    //        {
    //            wireReader = new WireXmlReader();
    //        }
    //        else if (type.Equals("DBF"))
    //        {
    //            wireReader = new WireDbfReader();
    //        }
    //        return wireReader;
    //    }
    //}

    public static class WireReader
    {
        public static List<WireType> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<WireType>();

            List<WireType> list = new List<WireType>();

            foreach (XmlNode typeNode in rootNode.ChildNodes)
            {
                WireType typeItem = new WireType()
                {
                    Type = typeNode.Attributes["Name"].Value.ToString(),
                    Wire = new List<Wire>()
                };

                foreach (XmlNode node in typeNode.ChildNodes)
                {
                    Wire wire = new Wire()
                    {
                        Name = node.Attributes["Name"].Value.ToString(),
                        Sec = Convert.ToInt16(node.Attributes["Sec"].Value.ToString()),
                        Dia = Convert.ToInt16(node.Attributes["Dia"].Value.ToString()),
                        Wei = Convert.ToInt16(node.Attributes["Wei"].Value.ToString()),
                        Elas = Convert.ToInt16(node.Attributes["Elas"].Value.ToString()),
                        Coef = Convert.ToInt16(node.Attributes["Coef"].Value.ToString()),
                        Fore = Convert.ToInt16(node.Attributes["Fore"].Value.ToString()),
                    };
                    typeItem.Wire.Add(wire);
                }
            }

            return list;
        }

        public static void Save(string path, List<WireType> infos)
        {
            XmlUtils.Save(path, infos);
        }

    }
}