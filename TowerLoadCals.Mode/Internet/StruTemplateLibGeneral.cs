using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// created by :glj
/// </summary>

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 结构模板库-通用模板实体类
    /// </summary>
    [SugarTable("kb_strutemplatelibgeneral")]
    public class StruTemplateLibGeneral
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 模板类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public string Content { get; set; }
    }

}
