/*
 * Сделано в SharpDevelop.
 * Пользователь: 055makarov
 * Дата: 20.08.2014
 * Время: 14:12
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace SilentInstall
{
	/// <summary>
	/// Description of SilentInstall.
	/// </summary>
	public class SilentInstall
	{
		
		private string _progFolder;
		private string _configFile;
		private List<SilentInstallationInfo> _instData;
		
		public event Action<string> SendInstallInfo;
		
		public SilentInstall(string progFolder,string configFile)
		{
			_instData=new List<SilentInstallationInfo>();
			_progFolder=progFolder;
			_configFile=configFile;
		}

		public void WaitKeyPressed()
		{
			bool exit = false;
			while(!exit)
			{
				ConsoleKeyInfo cki = Console.ReadKey(true);
				if(cki.Key==ConsoleKey.Escape)
				{
					exit=true;
				}
				else if(cki.Key==ConsoleKey.F1)
				{
					SilentInstallationAll();
				}
				else
				{
					int index;
					bool test =int.TryParse(cki.KeyChar.ToString(),out index);
					if(test==true && index<_instData.Count)
					{
						SilentInstallation(_instData[index]);
					}
				}		
			}
		}
		
		
		private void SilentInstallationAll()
		{
			foreach(SilentInstallationInfo sii in _instData)
			{
				SilentInstallation(sii);
			}
		}
		
		private void SilentInstallation(SilentInstallationInfo data)
		{
			if(SendInstallInfo!=null)
			{
				SendInstallInfo(string.Format("{0} - Installing",data.DisplayName));
			}
			
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.UseShellExecute=false;
			psi.FileName=data.ProgramPath;
			psi.Arguments=data.Args;
			Process p = Process.Start(psi);
			while(!p.HasExited)
			{
				Thread.Sleep(1000);
			}
			
			if(SendInstallInfo!=null)
			{
				if(p.ExitCode==0)
				{
					SendInstallInfo(string.Format("{0} - Complete",data.DisplayName));
				}
				else
				{
					SendInstallInfo(string.Format("{0} - Error",data.DisplayName));
				}
			}
		}
		
		public List<SilentInstallationInfo> ParseInstalationData()
		{
			string dataPath = Path.Combine(_progFolder,_configFile);
			using(StreamReader sr = new StreamReader(dataPath))
			{
				while(!sr.EndOfStream)
				{
					string data = sr.ReadLine();
					SilentInstallationInfo sii=null;
					if(data.Contains(".msi"))
					{
						sii=SilentInstallationInfo.ParseMsiData(Path.Combine(_progFolder,data.Trim()));
					}
					else if(data.Contains(".exe"))
					{
						sii=SilentInstallationInfo.ParseExeData(Path.Combine(_progFolder,data.Trim()));
					}
					
					if(sii!=null)
						_instData.Add(sii);
				}
			}
			return _instData;
		}
		
	}
}
