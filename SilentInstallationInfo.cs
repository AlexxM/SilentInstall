/*
 * Сделано в SharpDevelop.
 * Пользователь: 055makarov
 * Дата: 20.08.2014
 * Время: 15:33
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.IO;
namespace SilentInstall
{
	/// <summary>
	/// Description of SilentInstallationInfo.
	/// </summary>
	public class SilentInstallationInfo
	{
		string _displayName;
		string _programPath;
		string _args;	

		public string ProgramPath
		{
			get{return _programPath;}
		}
		
		public string Args
		{
			get{return _args;}
		}
		
		public string DisplayName
		{
			get{return _displayName;}
		}
			
		public SilentInstallationInfo(string programPath,string args,string displayName=null)
		{
			_programPath=programPath;
			_args=args;
			if(displayName==null)
				_displayName=_programPath;
			else
				_displayName=displayName;
		}
		
		public static SilentInstallationInfo ParseExeData(string str,char separator = ',')
		{
			string[] data = str.Split(separator);
			if(data.Length==2)
			{
				string fPath = Path.GetFullPath(data[0]);
				return new SilentInstallationInfo(fPath,data[1],data[0]);
			}
			else
			{
				return null;
			}
		}
		
		public static SilentInstallationInfo ParseMsiData(string str,char separator = ',')
		{
			string[] data = str.Split(separator);
			if(data.Length==2)
			{
				string fPath = '"'+Path.GetFullPath(data[0])+'"';
				string args = "/i " + fPath+" "+ data[1];
				return new SilentInstallationInfo("msiexec.exe",args,data[0]);
			}
			else
			{
				return null;
			}
		}
	
	}
}
