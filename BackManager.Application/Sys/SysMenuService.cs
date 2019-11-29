using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.Login;
using BackManager.Common.DtoModel.Model.RouterDto;
using BackManager.Domain;
using BackManager.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnitOfWork.Customer
{
    public class SysMenuService : ISysMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysMenu> _sysMenuRepository;

        public SysMenuService(IUnitOfWork unitOfWork, IRepository<SysMenu> sysUserRepository)
        {
            _sysMenuRepository = sysUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<ApiResult<long>> DeleteAsync(long[] ids)
        {
            throw new NotImplementedException();
        }







        public Task<ApiResult<long>> InsertAsync(SysMenuDto model)
        {
            throw new NotImplementedException();
        }



        public Task<ApiResult<long>> UpdateAsync(SysMenuDto model)
        {
            throw new NotImplementedException();
        }



        Task<ApiResult<SysMenuDto>> IDataEntityAsync<SysMenuDto>.GetModelAsync(long id)
        {
            throw new NotImplementedException();
        }

        async Task<ApiResult<PageResult<SysMenuDto>>> IDataEntityAsync<SysMenuDto>.GridInfoAsync<Par>(BackManager.Domain.DomainDrive.QueryParameter<Par> parameter)
        {
            List<SysMenu> dbSysMenus = await _sysMenuRepository.GetAllListAsync(m => m.DeleteFlag == Convert.ToInt32(EDeleteFlag.正常));
            List<SysMenuDto> sysMenuDtos = new List<SysMenuDto>();
            if (dbSysMenus != null)
            {
                List<SysMenuDto> dbSysMenuDtos = AutoMapperHelper.MapToList<SysMenu, SysMenuDto>(dbSysMenus).ToList();
                sysMenuDtos = SetSysMenuDto(0, dbSysMenuDtos, sysMenuDtos);
            }

            return ApiResult<PageResult<SysMenuDto>>.Ok(new PageResult<SysMenuDto>()
            {
                Rows = sysMenuDtos,
                Total = 0,
                PageTotal = 1
            });

        }



        private List<SysMenuDto> SetSysMenuDto(long FatherID, List<SysMenuDto> dbSysMenuDtos, List<SysMenuDto> sysMenuDtos)
        {

            if (dbSysMenuDtos.Any(m => m.FatherID == FatherID))
            {

                if (FatherID == 0)
                {
                    sysMenuDtos = dbSysMenuDtos.Where(m => m.FatherID == FatherID).ToList();
                    sysMenuDtos.ForEach(m =>
                    {
                        m.Children = SetSysMenuDto(m.ID, dbSysMenuDtos, sysMenuDtos);
                    });
                }
                else
                {
                    var Children = dbSysMenuDtos.Where(m => m.FatherID == FatherID).ToList();
                    Children.ForEach(m =>
                    {
                        m.Children = SetSysMenuDto(m.ID, dbSysMenuDtos, sysMenuDtos);
                    });
                    return Children.OrderBy(m => m.Orderby).ToList();
                }
            }

            return sysMenuDtos;
        }

        /// <summary>
        /// 菜单信息
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        public Task<RouterDto> GetUserMenuList(long iD)
        {
            List<SysMenu> sysMenuList = _sysMenuRepository.GetAll().Where(m => m.DeleteFlag == 0).Where(m =>
            !(new List<long> { 5, 6 }.Contains(m.ID))
            ).ToList();
            RouterDto result = new RouterDto
            {
                Data = new RouterInfo
                {
                    Router = new List<MenuInfo>()
                }
            };
            List<SysMenuDto> menuDtoList = AutoMapperHelper.MapToList<SysMenu, SysMenuDto>(sysMenuList).ToList();
            //构造递归集合
            List<SysMenuDto> res = Recursion(menuDtoList);

            List<MenuInfo> menuInfoList = BuilderVueRouter(res);
            result.Data.Router = menuInfoList;
            return Task.FromResult(result);
        }

        /// <summary>
        /// 构造菜单列表层级关系
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<SysMenuDto> Recursion(List<SysMenuDto> source)
        {
            List<SysMenuDto> result = new List<SysMenuDto>();
            foreach (SysMenuDto item in source.Where(w => w.FatherID == 0 || w.FatherID == null).OrderBy(m => m.Orderby))
            {
                result.Add(source.FirstOrDefault(x => x.ID == item.ID));
                Add(source, source.FirstOrDefault(x => x.ID == item.ID));
            }
            return result;
        }
        /// <summary>
        /// 递归添加
        /// </summary>
        /// <param name="list"></param>
        /// <param name="smd"></param>
        private void Add(List<SysMenuDto> list, SysMenuDto smd)
        {
            List<SysMenuDto> nextList = list.Where(w => w.FatherID == smd.ID).ToList();
            smd.Children = nextList;

            foreach (SysMenuDto item in nextList)
            {
                Add(list, item);
            }
        }

        /// <summary>
        /// 构造VUE路由数据
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<MenuInfo> BuilderVueRouter(List<SysMenuDto> source)
        {
            List<MenuInfo> result = new List<MenuInfo>();
            foreach (SysMenuDto menu in source)
            {
                MenuInfo dto = Builder(menu);
                result.Add(dto);
            }

            return result;
        }
        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private MenuInfo Builder(SysMenuDto menu)
        {
            //如果是顶级菜单,并且没有子集,需要单独构造,把当前菜单写在 Children 里面
            if (menu.FatherID == 0 && (menu.Children == null || menu.Children.Count == 0))
            {
                return new MenuInfo
                {
                    Path = GetRoutePath(menu.Url),
                    Component = "Layout",
                    Redirect = string.Empty,
                    Children = new List<MenuInfo>
                    {
                        new MenuInfo
                        {
                            Path = PinyinHelper.GetFirstPinyin(menu.Name),
                            Name = menu.Name,
                            Component = menu.Url,
                            Meta = new Meta
                            {
                                Icon = menu.Icon,
                                Title = menu.Name,
                                MenuID = menu.ID,
                            },
                        }
                    }
                };
            }

            MenuInfo result = new MenuInfo
            {
                Component = menu.Url,
                Name = menu.Name,
                Redirect = string.Empty,
                Meta = new Meta
                {
                    Icon = menu.Icon,
                    Title = menu.Name,
                    MenuID = menu.ID,
                },
                Path = menu.Path
            };

            List<MenuInfo> childrenList = new List<MenuInfo>();
            if (menu.Children.CheckNotEmpty() == true)
            {
                foreach (SysMenuDto menuChild in menu.Children)
                {
                    childrenList.Add(Builder(menuChild));
                }
            }
            result.Children = childrenList;

            //if (result.Children.Count == 0)
            //{
            //    result.Path = PinyinHelper.GetFirstPinyin(result.Name);
            //}
            //else
            //{
            //    result.Component = "Layout";
            //    result.Path = GetRoutePath(menu.Url);
            //}


            if (result.Children.Count > 0)
            {
                result.Component = "Layout";
                result.Path = GetRoutePath(menu.Url);
            }
            return result;
        }

        /// <summary>
        /// 根据菜单url获取前端路由需要的 Path
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetRoutePath(string url)
        {
            string[] arr = url.Split('/');
            if (arr.Length >= 2)
            {
                return "/" + arr[1];
            }
            return string.Empty;
        }

        public async Task<ApiResult<long>> saveMenuSort(Dictionary<int, int> dicSort)
        {
            List<SysMenu> sysMenus = new List<SysMenu>();
            foreach (KeyValuePair<int, int> item in dicSort)
            {
                sysMenus.Add(new SysMenu { ID = item.Key, Orderby = item.Value });
            }

            int updates = await _sysMenuRepository.BulkUpdate(sysMenus,
                   m => new
                   {
                       m.Orderby
                   },
                  m => new
                  {
                      m.ID
                  });

            return ApiResult<long>.Ok(1);

        }
    }


}