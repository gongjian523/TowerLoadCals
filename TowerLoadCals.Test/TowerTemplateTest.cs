using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.DAL.Structure;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Test
{
    [TestClass]
    public class TowerTemplateTest
    {
        [TestMethod]
        public void TestMethod1_NewTemplateReadAndSave()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL Files (*.dll)|*.dll"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            string datPath = openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 3) + "dat";

            DES.DesDecrypt(openFileDialog.FileName, datPath, "12345678");


            TowerTemplateReader templateReader = new TowerTemplateReader(TowerTypeEnum.LineTower);
            TowerTemplate template = templateReader.Read(datPath);


            string newDatPath = openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 4) + "New.dat";
            NewTowerTemplateReader newTemplateReader = new NewTowerTemplateReader(TowerTypeEnum.LineTower);
            newTemplateReader.Save(newDatPath, template);

            TowerTemplate newTemplate = newTemplateReader.Read(newDatPath);

        }

        [TestMethod]
        public void TestMethod2_SmartTowerGenerator()
        {
            var openLoadDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Load Files (*.load)|*.load"
            };

            if (openLoadDialog.ShowDialog() != true)
                return;

            var openTemplateDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Dat Files (*.dat)|*.dat"
            };

            if (openTemplateDialog.ShowDialog() != true)
                return;

            string rlt = SmartTowerInputGenerator.InputGenerator(openLoadDialog.FileName, openTemplateDialog.FileName);
        }
    }
}
