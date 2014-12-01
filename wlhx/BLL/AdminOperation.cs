using BLL;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wlhx.Common;
using wlhx.Models;

namespace wlhx.BLL
{
    public class AdminOperation : BaseBLL<Admin>
    {
        public string GetAdminJson()
        {
            string json = "[";
            List<Admin> admins = base.Search(d => d.admin_power != 2 && d.admin_isDel == false);
            foreach (Admin admin in admins)
            {
                json = json + "{\"id\":\"" + admin.admin_id + "\",\"user\":\"" + admin.admin_username + "\",\"level\":\"" + GetAdminLevel(admin.admin_power) + "\"},";
            }
            json = json.Substring(0, json.Length - 1) + "]";
            return json;
        }

        public string GetAdminLevel(int power)
        {
            return power == 0 ? "超级管理员" : "管理员";
        }

        public JsonStatus ModifyAdmin(string id, string user, string level)
        {
            long adminId = -1;
            int power = -1;
            JsonStatus js = new JsonStatus();
            try
            {
                adminId = Convert.ToInt64(id);
                power = Convert.ToInt32(level);
                if (CheckDataOperation.CheckData(user, @"^[A-Za-z0-9]{0,25}$") && (power == 0 || power == 1))
                {
                    List<Admin> admins = base.Search(d => d.admin_id == adminId && d.admin_isDel == false);
                    if (admins.Count == 1)
                    {
                        admins[0].admin_power = power;
                        admins[0].admin_username = user;
                        base.Modify(admins[0], new string[] { "admin_power", "admin_username" });
                        js.status = "1";
                        js.msg = "修改成功!";
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "数据出现异常.....";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "数据不合法.....";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "数据出现异常.....";
            }

            return js;
        }

        public JsonStatus RemoveAdmin(string id)
        {
            long adminId = -1;
            JsonStatus js = new JsonStatus();
            try
            {
                adminId = Convert.ToInt64(id);
                List<Admin> admins = base.Search(d => d.admin_isDel == false && d.admin_id == adminId);
                if (admins.Count == 1)
                {
                    admins[0].admin_isDel = true;
                    base.Modify(admins[0], "admin_isDel");
                    js.status = "1";
                    js.msg = "删除成功!";
                }
                else
                {
                    js.status = "0";
                    js.msg = "数据出现异常.....";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "数据出现异常.....";
            }
            return js;
        }

        public JsonStatus ResetPassword(string id)
        {
            long adminId = -1;
            JsonStatus js = new JsonStatus();
            try
            {
                adminId = Convert.ToInt64(id);
                List<Admin> admins = base.Search(d => d.admin_isDel == false && d.admin_id == adminId);
                if (admins.Count == 1)
                {
                    admins[0].admin_password = Md5.GetMd5Word("123456", "123456");
                    base.Modify(admins[0], "admin_password");
                    js.status = "1";
                    js.msg = "重置成功!";
                }
                else
                {
                    js.status = "0";
                    js.msg = "数据出现异常.....";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "数据出现异常.....";
            }
            return js;
        }

        public JsonStatus AddAdmin(string user, string level)
        {
            int power = -1;
            JsonStatus js = new JsonStatus();
            try
            {
                power = Convert.ToInt32(level);
                if (CheckDataOperation.CheckData(user, @"^[A-Za-z0-9]{0,25}$") && (power == 0 || power == 1))
                {
                    Admin a = new Admin()
                    {
                        admin_id = 1,
                        admin_isDel = false,
                        admin_password = Md5.GetMd5Word("123456", "123456"),
                        admin_power = power,
                        admin_username = user
                    };
                    base.Add(a);
                    js.status = "1";
                    js.msg = "添加成功!";

                }
                else
                {
                    js.status = "0";
                    js.msg = "数据不合法.....";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "数据出现异常.....";
            }

            return js;
        }

        public JsonStatus RePasswordIn(long id, string jpwd, string xpwd, string qpwd)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                Admin a = base.Search(d => d.admin_id == id && d.admin_isDel == false)[0];

                if (CheckDataOperation.CheckData(xpwd, @"^[A-Za-z0-9]{0,25}$"))
                {
                    if (xpwd == qpwd)
                    {
                        a.admin_password =Md5.GetMd5Word(xpwd,xpwd);
                        base.Modify(a, "admin_password");
                        js.status = "1";
                        js.msg = "修改成功!";
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "两次输入的密码不同.....";
                    }

                }
                else
                {
                    js.status = "0";
                    js.msg = "数据不合法.....";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "数据出现异常.....";
            }

            return js;
        }

        public JsonStatus Login(string user, string pwd, string yzm, string sys_yzm,out long id)
        {
            JsonStatus js=new JsonStatus();
            if (yzm == sys_yzm)
            {
                pwd = Md5.GetMd5Word(pwd, pwd);
                List<Admin> admins = base.Search(d => d.admin_username == user && d.admin_isDel == false && d.admin_password == pwd);
                if (admins.Count >= 1)
                {
                    id = admins[0].admin_id;
                    js.status = "1";
                    js.msg = "登陆成功！";
                }
                else
                {
                    id = 0;
                    js.status = "0";
                    js.msg = "用户名或密码错误....";
                }
            }
            else
            {
                id = 0;
                js.status = "0";
                js.msg = "验证码错误....";
            }
            return js;
        }
    }
}