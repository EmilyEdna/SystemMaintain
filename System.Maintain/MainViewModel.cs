using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Maintain.InputWindow;
using System.Maintain.NotifyModel;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace System.Maintain
{
    public class MainViewModel : ObservableObject
    {

        public RichTextBox RichBox { get; set; }
        private int Limit = 4;
        public MainViewModel()
        {
            CommandLine.CmdLog = new(log =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var P = new Paragraph();
                    P.Inlines.Add(new Run(log));
                    if (RichBox != null)
                        RichBox.Document.Blocks.Add(P);
                });
            });
            IIS = new RelayCommand(IISCommand);
            Runtime = new RelayCommand(RuntimeCommand);
            Proxy = new RelayCommand(ProxyCommand);
            Function = new RelayCommand<HandleEnum>(HandlerCommand);
            Excutor = new RelayCommand<string>(RunCmdCommand);
            Clear = new RelayCommand(ClearCommand);
        }

        #region Property
        private ObservableCollection<NavbarNotifyModel> _NavBar;
        public ObservableCollection<NavbarNotifyModel> NavBar
        {
            get => _NavBar;
            set => SetProperty(ref _NavBar, value);
        }
        #endregion

        #region 命令
        private void RuntimeCommand()
        {
            NavBar = new ObservableCollection<NavbarNotifyModel>
            {
                new NavbarNotifyModel{ Key=HandleEnum.FrameWork3_5,Value="①安装.NetFramework3.5" },
                new NavbarNotifyModel{ Key=HandleEnum.NET6,Value="②安装.Net6" },
                new NavbarNotifyModel{ Key=HandleEnum.DirectX,Value="③安装DirectX" },
                new NavbarNotifyModel{ Key=HandleEnum.Redis,Value="④安装Redis" },
                new NavbarNotifyModel{ Key=HandleEnum.Redis,Value="⑤安装WPS" },
            };
        }
        private void ProxyCommand()
        {
            NavBar = new ObservableCollection<NavbarNotifyModel>
            {
                new NavbarNotifyModel{ Key=HandleEnum.Nignx_CMP,Value="①注册Nignx[CMP]配置" },
                new NavbarNotifyModel{ Key=HandleEnum.Nignx_CCRT,Value="②注册Nignx[CCRT]配置" },
                new NavbarNotifyModel{ Key=HandleEnum.Nignx_MATS,Value="③注册Nignx[MATS]配置" },
                new NavbarNotifyModel{ Key=HandleEnum.Nignx_MECT,Value="④注册Nignx[MECT]配置" },
                new NavbarNotifyModel{ Key=HandleEnum.Nignx_ALL,Value="⑤注册Nignx全部配置" },
                new NavbarNotifyModel{ Key=HandleEnum.Nignx,Value="⑥安装Nignx" },
                new NavbarNotifyModel{ Key=HandleEnum.NignxRe,Value="⑦重启Nignx" },
            };
        }

        private void IISCommand()
        {
            NavBar = new ObservableCollection<NavbarNotifyModel>
            {
                new NavbarNotifyModel{ Key=HandleEnum.IIS,Value="①安装IIS" },
                new NavbarNotifyModel{ Key=HandleEnum.CMP,Value="②安装CMP产品" },
                new NavbarNotifyModel{ Key=HandleEnum.CCRT,Value="③安装CCRT产品" },
                new NavbarNotifyModel{ Key=HandleEnum.MATS,Value="④安装MATS产品" },
                new NavbarNotifyModel{ Key=HandleEnum.MECT,Value="⑤安装MECT产品" },
                new NavbarNotifyModel{ Key=HandleEnum.ALL,Value="⑥安装全部产品" },
                new NavbarNotifyModel{ Key=HandleEnum.DelALL,Value="⑦删除全部产品" }
            };
        }
        private void HandlerCommand(HandleEnum obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("c: \n");
            sb.Append("cd C:\\Windows\\System32\\inetsrv \n");
            switch (obj)
            {
                case HandleEnum.IIS:
                    IISInstall();
                    break;
                case HandleEnum.CMP:
                    for (int i = 1; i <= Limit; i++)
                    {
                        sb.Append(ApiInstall("cmpapi", i, 9980 + i));
                    }
                    ExCutorBat(sb, "CMP");
                    break;
                case HandleEnum.CCRT:
                    for (int i = 1; i <= Limit; i++)
                    {
                        sb.Append(ApiInstall("cmpapi", i, 9980 + i));
                        sb.Append(ApiInstall("crtapi", i, 9970 + i));
                    }
                    sb.Append(ApiInstall("crtui", 1, 9979));
                    sb.Append(ApiInstall("crtapp", 1, 9978));
                    ExCutorBat(sb, "CCRT");
                    break;
                case HandleEnum.MATS:
                    for (int i = 1; i <= Limit; i++)
                    {
                        sb.Append(ApiInstall("cmpapi", i, 9980 + i));
                        sb.Append(ApiInstall("matsapi", i, 9960 + i));
                    }
                    ExCutorBat(sb, "MATS");
                    break;
                case HandleEnum.MECT:
                    for (int i = 1; i <= Limit; i++)
                    {
                        sb.Append(ApiInstall("ectapi", i, 9950 + i));
                    }
                    ExCutorBat(sb, "MECT");
                    break;
                case HandleEnum.ALL:
                    for (int i = 1; i <= Limit; i++)
                    {
                        sb.Append(ApiInstall("cmpapi", i, 9980 + i));
                        sb.Append(ApiInstall("crtapi", i, 9970 + i));
                        sb.Append(ApiInstall("matsapi", i, 9960 + i));
                        sb.Append(ApiInstall("ectapi", i, 9950 + i));
                    }
                    sb.Append(ApiInstall("crtui", 1, 9979));
                    sb.Append(ApiInstall("crtapp", 1, 9978));
                    ExCutorBat(sb, "AllProduct");
                    break;
                case HandleEnum.DelALL:
                    for (int i = 1; i <= Limit; i++)
                    {
                        sb.Append(DeleteIIS(i));
                    }
                    ExCutorBat(sb, "UnInstall");
                    break;
                case HandleEnum.Nignx_CMP:
                    BuildNignxFile(HandleEnum.Nignx_CMP);
                    break;
                case HandleEnum.Nignx_CCRT:
                    BuildNignxFile(HandleEnum.Nignx_CCRT);
                    break;
                case HandleEnum.Nignx_MATS:
                    BuildNignxFile(HandleEnum.Nignx_MATS);
                    break;
                case HandleEnum.Nignx_MECT:
                    BuildNignxFile(HandleEnum.Nignx_MECT);
                    break;
                case HandleEnum.Nignx_ALL:
                    BuildNignxFile(HandleEnum.Nignx_ALL);
                    break;
                case HandleEnum.Nignx:
                    var NignxStr = sb.Append(NignxBuild());
                    CommandLine.P.StandardInput.WriteLine(NignxStr);
                    break;
                case HandleEnum.NignxRe:
                    break;
                case HandleEnum.Redis:
                    CommandLine.P.StandardInput.WriteLine("cd runtimes");
                    CommandLine.P.StandardInput.WriteLine("Redis-x64-3.2.100.msi  /passive");
                    break;
                case HandleEnum.FrameWork3_5:
                    CommandLine.P.StandardInput.WriteLine("cd runtimes");
                    break;
                case HandleEnum.NET6:
                    CommandLine.P.StandardInput.WriteLine("cd runtimes");
                    CommandLine.P.StandardInput.WriteLine("dotnet-hosting-6.0.11-win.exe  /passive");
                    break;
                case HandleEnum.DirectX:
                    CommandLine.P.StandardInput.WriteLine("cd runtimes");
                    CommandLine.P.StandardInput.WriteLine("DirectX修复工具3.9.exe /passive");
                    break;
                case HandleEnum.WPS:
                    CommandLine.P.StandardInput.WriteLine("cd runtimes");
                    CommandLine.P.StandardInput.WriteLine("W.P.S.10495.12012.2019.exe /passive");
                    break;
                case HandleEnum.MySql:
                    break;
                default:
                    break;
            }
        }
        private void RunCmdCommand(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
                CommandLine.P.StandardInput.WriteLine(obj);
        }
        private void ClearCommand()
        {
            RichBox.Document.Blocks.Clear();
        }

        public ICommand Function { get; }
        public ICommand IIS { get; }
        public ICommand Excutor { get; }
        public ICommand Runtime { get; }
        public ICommand Proxy { get; }
        public ICommand Clear { get; }
        #endregion

        #region IIS 方法
        string DeleteIIS(int index)
        {
            StringBuilder sb = new StringBuilder();
            //删除网站
            sb.Append($"appcmd.exe delete site sysapi{index} \n");
            sb.Append($"appcmd.exe delete site cmpapi{index} \n");
            sb.Append($"appcmd.exe delete site crtapi{index} \n");
            sb.Append($"appcmd.exe delete site matsapi{index} \n");
            sb.Append($"appcmd.exe delete site ectapi{index} \n");
            //删除站点
            sb.Append($"appcmd.exe delete apppool  sysapi{index} \n");
            sb.Append($"appcmd.exe delete apppool  cmpapi{index} \n");
            sb.Append($"appcmd.exe delete apppool  crtapi{index} \n");
            sb.Append($"appcmd.exe delete apppool  matsapi{index} \n");
            sb.Append($"appcmd.exe delete apppool  ectapi{index} \n");
            //clickonce发布
            sb.Append($"appcmd.exe delete site  crtui{index} \n");
            sb.Append($"appcmd.exe delete site  crtapp{index} \n");
            sb.Append($"appcmd.exe delete apppool  crtui{index} \n");
            sb.Append($"appcmd.exe delete apppool  crtapp{index} \n");
            return sb.ToString();
        }
        string ApiInstall(string name, int index, int Port)
        {
            StringBuilder sb = new StringBuilder();
            //配置程序池
            sb.Append($"appcmd add apppool /name:{name}{index} /managedRuntimeVersion:  /managedPipelineMode:Integrated \n");
            sb.Append($"appcmd.exe set apppool {name}{index} /processModel.identityType:LocalSystem \n");
            sb.Append($"appcmd.exe set apppool {name}{index} /config /StartMode:AlwaysRunning \n");
            sb.Append($"appcmd.exe set apppool {name}{index} /config /+Recycling.PeriodicRestart.Schedule.[Value='04:00:00'] \n");
            sb.Append($"appcmd.exe set apppool {name}{index} /Recycling.PeriodicRestart.time:00:00:00 \n");
            sb.Append($"appcmd.exe set apppool {name}{index} /ProcessModel.ShutdownTimeLimit:00:02:00 \n");
            sb.Append($"appcmd.exe set apppool {name}{index} /ProcessModel.StartupTimeLimit:00:02:00 \n");
            sb.Append($"appcmd.exe set apppool {name}{index} /ProcessModel.IdleTimeoutAction:Suspend \n");
            //创建服务
            sb.Append($"appcmd add site /name:{name}{index} /bindings:http/*:{Port}: /physicalPath:{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"publish\\jjwf\\{name}")} \n");
            sb.Append($"appcmd set site /site.name:{name}{index} /[path='/'].applicationPool:{name}{index} \n");
            sb.Append($"appcmd set site /site.name:{name}{index} /ApplicationDefaults.PreloadEnabled:true \n");
            return sb.ToString();
        }
        void IISInstall()
        {

            InputView input = new InputView("请输入Hosts映射地址");
            if (input.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(input.Inputs.Text))
                {
                    CommandLine.P.StandardInput.WriteLine($"echo {input.Inputs.Text} www.jjwf.com>>C:\\Windows\\System32\\drivers\\etc\\hosts");
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("c: \n");
            sb.Append("cd C:\\Windows\\System32\\inetsrv \n");
            sb.Append("dism.exe /Online /Enable-Feature /FeatureName:IIS-WebServerRole /FeatureName:IIS-WebServer /FeatureName:IIS-ManagementService /FeatureName:IIS-WebServerManagementTools /FeatureName:IIS-HostableWebCore /FeatureName:IIS-ApplicationInit /NoRestart \n");
            for (int i = 1; i <= Limit; i++)
            {
                sb.Append(DeleteIIS(i));
            }
            for (int i = 1; i <= Limit; i++)
            {
                sb.Append(ApiInstall("sysapi", i, 9990 + i));
            }
            ExCutorBat(sb, "Install");
        }

        void ExCutorBat(StringBuilder sb, string filename)
        {
            var batch = sb.Append("exit 0 \n").ToString();
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename + ".bat");
            if (File.Exists(file) == true) File.Delete(file);
            File.Create(file).Dispose();
            File.WriteAllBytes(file, Encoding.Default.GetBytes(batch));
            CommandLine.CmdBat(file);
        }
        #endregion

        #region Nignx
        string NignxNode(HandleEnum handle) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("upstream sysapinode \n { \n least_conn;");
            for (int index = 1; index <= 4; index++)
            {
                sb.Append($"\n server www.jjwf.com:{9990 + index};");
            }
            sb.Append("\n } \n");

            if (handle == HandleEnum.Nignx_CMP)
            {
                //cmp
                sb.Append("upstream cmpapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9980 + index};");
                }
                sb.Append("\n } \n");
            }

            if (handle == HandleEnum.CCRT)
            {
                //cmp
                sb.Append("upstream cmpapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9980 + index};");
                }
                sb.Append("\n } \n");
                //ccrt
                sb.Append("upstream crtapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9970 + index};");
                }
                sb.Append("\n } \n");
                sb.Append("upstream  crtuinode \n { \n least_conn; \n server www.jjwf.com:9979 \n }\n");
                sb.Append("upstream  crtappnode \n { \n least_conn; \n server www.jjwf.com:9978 \n }\n");
            }

            if (handle == HandleEnum.Nignx_MATS)
            {
                //cmp
                sb.Append("upstream cmpapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9980 + index};");
                }
                sb.Append("\n } \n");

                //mats
                sb.Append("upstream matsapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9960 + index};");
                }
                sb.Append("\n } \n");
            }

            if (handle == HandleEnum.Nignx_MECT)
            {
                //mect
                sb.Append("upstream mectapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9950 + index};");
                }
                sb.Append("\n } \n");
            }

            if (handle == HandleEnum.Nignx_ALL)
            {
                //cmp
                sb.Append("upstream cmpapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9980 + index};");
                }
                sb.Append("\n } \n");

                //ccrt
                sb.Append("upstream crtapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9970 + index};");
                }
                sb.Append("\n } \n");
                sb.Append("upstream  crtuinode \n { \n least_conn; \n server www.jjwf.com:9979 \n }\n");
                sb.Append("upstream  crtappnode \n { \n least_conn; \n server www.jjwf.com:9978 \n }\n");


                //mats
                sb.Append("upstreammatsapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9960 + index};");
                }
                sb.Append("\n } \n");

                //mect
                sb.Append("upstream mectapinode \n { \n least_conn;");
                for (int index = 1; index <= 4; index++)
                {
                    sb.Append($"\n server www.jjwf.com:{9950 + index};");
                }
                sb.Append("\n } \n");
            }

           return sb.ToString();
        }
        string NignxHttp(HandleEnum handle)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"
        location ^~ /sysapi/ {
            rewrite ^.+sysapi/?(.*)$ /$1 break;
            proxy_pass http://sysapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
	        proxy_set_header   X-Forwarded-Prefix sysapi;
        }
");

            if (handle == HandleEnum.Nignx_CMP)
            {
                sb.Append(@"
        location ^~ /cmpapi/ {
            rewrite ^.+cmpapi/?(.*)$ /$1 break;
            proxy_pass http://cmpapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix cmpapi;
        }
");
            }

            if (handle == HandleEnum.Nignx_CCRT)
            {
                sb.Append(@"
        location ^~ /cmpapi/ {
            rewrite ^.+cmpapi/?(.*)$ /$1 break;
            proxy_pass http://cmpapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix cmpapi;
        }
");

                sb.Append(@"
       location ^~ /crtapi/ {
            rewrite ^.+crtapi/?(.*)$ /$1 break;
            proxy_pass http://crtapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix crtapi;
        }
");

                sb.Append(@"
    location / {
            index publish.html index.html index.htm;
            proxy_pass http://crtuinode/;
            }
");

                sb.Append(@"
    location /train/ {
            index publish.html index.html index.htm;
            proxy_pass http://crtappnode/;
        }
");
            }

            if (handle == HandleEnum.Nignx_MATS)
            {
                sb.Append(@"
        location ^~ /cmpapi/ {
            rewrite ^.+cmpapi/?(.*)$ /$1 break;
            proxy_pass http://cmpapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix cmpapi;
        }
");

                sb.Append(@"
        location ^~ /matspapi/ {
            rewrite ^.+matspapi/?(.*)$ /$1 break;
            proxy_pass http://matsapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix matspapi;
        }
");
            }

            if (handle == HandleEnum.Nignx_MECT)
            {
                sb.Append(@"
        location ^~ /mectapi/ {
            rewrite ^.+mectapi/?(.*)$ /$1 break;
            proxy_pass http://mectapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix mectapi;
        }
");
            }

            if (handle == HandleEnum.Nignx_ALL)
            {
                sb.Append(@"
        location ^~ /cmpapi/ {
            rewrite ^.+cmpapi/?(.*)$ /$1 break;
            proxy_pass http://cmpapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix cmpapi;
        }
");

                sb.Append(@"
       location ^~ /crtapi/ {
            rewrite ^.+crtapi/?(.*)$ /$1 break;
            proxy_pass http://crtapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix crtapi;
        }
");

                sb.Append(@"
        location ^~ /matspapi/ {
            rewrite ^.+matspapi/?(.*)$ /$1 break;
            proxy_pass http://matsapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix matspapi;
        }
");

                sb.Append(@"
        location ^~ /mectapi/ {
            rewrite ^.+mectapi/?(.*)$ /$1 break;
            proxy_pass http://mectapinode;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Prefix mectapi;
        }
");

                sb.Append(@"
    location / {
            index publish.html index.html index.htm;
            proxy_pass http://crtuinode/;
            }
");

                sb.Append(@"
    location /train/ {
            index publish.html index.html index.htm;
            proxy_pass http://crtappnode/;
        }
");
            }
            return sb.ToString();
        }
        void BuildNignxFile(HandleEnum handle) 
        {
            var conf = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nginx-1.20.2","conf", "nginx.conf");
            if (File.Exists(conf) == true) File.Delete(conf);
            File.Create(conf).Dispose();
            using StreamWriter sw = new StreamWriter(conf, true);

            var Config =  NignxConf.Template.Replace("{0}", NignxNode(handle)).Replace("{1}", NignxHttp(handle));

            sw.Write(Config);
        }
        string NignxBuild()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("d: \n");
            sb.Append($"cd {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nginx-1.20.2")} \n");
            sb.Append("net stop nginx \n");
            sb.Append("sc delete nginx \n");
            sb.Append("reg add HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\services\\HTTP /v \"Start\" /t REG_DWORD /d 0 /f \n");
            sb.Append("nginx-service.exe install \n");
            sb.Append("net start nginx \n");
            return sb.ToString();
        }
        #endregion

    }
}
