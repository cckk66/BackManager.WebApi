using Newtonsoft.Json;
using System.Collections.Generic;

namespace BackManager.Common.DtoModel
{
    public class GroupMenuPowerDto
    {
        public long GroupID { get; set; }
        public List<GroupMenuDto> GroupMenuDtos { get; set; }
    }
    public class GroupMenuDto
    {
        public long ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty("label")]
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
        /// <summary>
        /// 节点唯一键
        /// </summary>
        [JsonProperty("nodeKey")]
        public string NodeKey
        {
            get
            {
                return GroupMenuDto.GetNodeKey(this.ID, this.GroupMenuType);
            }
        }
        public static string GetMenuNodeKey(long ID) => GroupMenuDto.GetNodeKey(ID, EGroupMenuType.Menu);
       
        public static string GetMenuButtonNodeKey(long ID) => GroupMenuDto.GetNodeKey(ID, EGroupMenuType.MenuButton);
    
        public static string GetNodeKey(long ID, EGroupMenuType eGroupMenuType)
        {
            return $"{System.Convert.ToInt32(eGroupMenuType)}_{ID}";
        }

    }
    public enum EGroupMenuType
    {
        Menu = 1,
        MenuButton = 2
    }
}
