using Newtonsoft.Json;
using System.Collections.Generic;

namespace BackManager.Common.DtoModel
{
    public class GroupMenuDto
    {
        public long ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>

        public string Name { get; set; }


        /// <summary>
        /// 菜单图标
        /// </summary>

        public string Icon { get; set; }


        /// <summary>
        /// 父节点ID
        /// </summary>
        public long? FatherID { get; set; }





        /// <summary>
        /// 排序
        /// </summary>
        public int? Orderby { get; set; }



        /// <summary>
        /// 下级菜单集合
        /// </summary>
        [JsonProperty("children")]
        public List<GroupMenuDto> Children { get; set; }

        public EGroupMenuType GroupMenuType { get; set; } = EGroupMenuType.Menu;
        public int SonCount { get; set; } = 0;
    }
    public enum EGroupMenuType
    {
        Menu = 1,
        MenuButton = 2
    }
}
