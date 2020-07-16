using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
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


            TowerTemplateReader templateReader = new TowerTemplateReader(TowerType.LineTower);
            TowerTemplate template = templateReader.Read(datPath);


            string newDatPath = openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 4) + "New.dat";
            NewTowerTemplateReader newTemplateReader = new NewTowerTemplateReader(TowerType.LineTower);
            newTemplateReader.Save(newDatPath, template);

            TowerTemplate newTemplate = newTemplateReader.Read(newDatPath);

        }
    }
}
