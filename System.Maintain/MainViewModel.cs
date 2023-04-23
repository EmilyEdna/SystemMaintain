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
            Function = new RelayCommand<int>(HandlerCommand);
            Excutor = new RelayCommand<string>(RunCmdCommand);
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
                new NavbarNotifyModel{ Key=1,Value="①安装.NetFramework3.5" },
                new NavbarNotifyModel{ Key=2,Value="②安装.Net6" },
                new NavbarNotifyModel{ Key=3,Value="③安装DirectX" },
                new NavbarNotifyModel{ Key=4,Value="④安装Redis" },
            };
        }
        private void IISCommand()
        {
            NavBar = new ObservableCollection<NavbarNotifyModel>
            {
                new NavbarNotifyModel{ Key=0,Value="清屏" },
                new NavbarNotifyModel{ Key=1,Value="①安装IIS" },
                new NavbarNotifyModel{ Key=2,Value="②安装CMP产品" },
                new NavbarNotifyModel{ Key=3,Value="③安装CCRT产品" },
                new NavbarNotifyModel{ Key=4,Value="④安装MATS产品" },
                new NavbarNotifyModel{ Key=5,Value="⑤安装MECT产品" },
                new NavbarNotifyModel{ Key=6,Value="⑥安装全部产品" },
                new NavbarNotifyModel{ Key=7,Value="⑦删除全部产品" }
            };
        }
        private void HandlerCommand(int obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("c: \n");
            sb.Append("cd C:\\Windows\\System32\\inetsrv \n");
            if (obj == 0) RichBox.Document.Blocks.Clear();
            if (obj == 1) IISInstall();
            if (obj == 2)
            {
                for (int i = 1; i <= Limit; i++)
                {
                    sb.Append(ApiInstall("cmpapi", i));

                    sb.Append(StartService("cmpapi", i));
                }
                ExCutorBat(sb, "CMP");
            }
            if (obj == 3)
            {
                for (int i = 1; i <= Limit; i++)
                {
                    sb.Append(ApiInstall("cmpapi", i));
                    sb.Append(ApiInstall("crtapi", i));

                    sb.Append(StartService("cmpapi", i));
                    sb.Append(StartService("crtapi", i));
                }
                sb.Append(ApiInstall("crtui", 1));
                sb.Append(ApiInstall("crtapp", 1));

                sb.Append(StartService("crtui", 1));
                sb.Append(StartService("crtapp", 1));
                ExCutorBat(sb, "CCRT");
            }
            if (obj == 4)
            {
                for (int i = 1; i <= Limit; i++)
                {
                    sb.Append(ApiInstall("cmpapi", i));
                    sb.Append(ApiInstall("matsapi", i));

                    sb.Append(StartService("cmpapi", i));
                    sb.Append(StartService("matsapi", i));
                }
                ExCutorBat(sb, "MATS");
            }
            if (obj == 5)
            {

                for (int i = 1; i <= Limit; i++)
                {
                    sb.Append(ApiInstall("ectapi", i));

                    sb.Append(StartService("ectapi", i));
                }
                ExCutorBat(sb, "MECT");
            }
            if (obj == 6)
            {

                for (int i = 1; i <= Limit; i++)
                {
                    sb.Append(ApiInstall("cmpapi", i));
                    sb.Append(ApiInstall("crtapi", i));
                    sb.Append(ApiInstall("matsapi", i));
                    sb.Append(ApiInstall("ectapi", i));

                    sb.Append(StartService("cmpapi", i));
                    sb.Append(StartService("crtapi", i));
                    sb.Append(StartService("matsapi", i));
                    sb.Append(StartService("ectapi", i));
                }
                sb.Append(ApiInstall("crtui", 1));
                sb.Append(ApiInstall("crtapp", 1));

                sb.Append(StartService("crtui", 1));
                sb.Append(StartService("crtapp", 1));
                ExCutorBat(sb, "AllProduct");
            }
            if (obj == 7)
            {
                for (int i = 1; i <= Limit; i++)
                {
                    sb.Append(DeleteIIS(i));
                }
                ExCutorBat(sb, "UnInstall");
            }
        }
        private void RunCmdCommand(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
                CommandLine.P.StandardInput.WriteLine(obj);
        }
        public ICommand Function { get; }
        public ICommand IIS { get; }
        public ICommand Excutor { get; }
        public ICommand Runtime { get; }
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
        string ApiInstall(string name, int index)
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
            sb.Append($"appcmd add site /name:{name}{index} /bindings:http/*:999{index}: /physicalPath:{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"publish\\jjwf\\{name}")} \n");
            sb.Append($"appcmd set site /site.name:{name}{index} /[path='/'].applicationPool:{name}{index} \n");
            sb.Append($"appcmd set site /site.name:{name}{index} /ApplicationDefaults.PreloadEnabled:true \n");
            return sb.ToString();
        }
        string StartService(string name, int index) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"appcmd.exe start apppool {name}{index} \n");
            sb.Append($"appcmd.exe start site {name}{index} \n");
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
                sb.Append(ApiInstall("sysapi", i));
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

    }
}
