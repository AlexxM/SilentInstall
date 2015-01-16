/*
 * Сделано в SharpDevelop.
 * Пользователь: 055makarov
 * Дата: 18.08.2014
 * Время: 15:01
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Threading;
namespace SilentInstall
{
	class Program
	{

		public static void Main(string[] args)
		{
			try{
			System.ComponentModel.ComponentResourceManager cmr = new System.ComponentModel.ComponentResourceManager(typeof(Program));
			SilentInstall si  = new SilentInstall(cmr.GetString("progFolder"),cmr.GetString("configFile"));
			si.SendInstallInfo+= SendInstallInfoHandler;
			List<SilentInstallationInfo> sii = si.ParseInstalationData();
			ShowPrograms(sii);
			si.WaitKeyPressed();
			}
			catch(Exception ex)
			{
				Console.WriteLine("Ошибка в работе программы");
				Console.ReadLine();
			}
			return;
		}
		
		
		public static void SendInstallInfoHandler(string s)
		{
			Console.WriteLine(s);
		}
		
		
		public static void ShowPrograms(List<SilentInstallationInfo> instData)
		{
			int cnt = 0;
			Console.WriteLine("Start this program with administrative rules!");
			Console.WriteLine("Please select program...");
			foreach(SilentInstallationInfo i in instData)
			{
				Console.WriteLine("{0} - {1} ",cnt,i.DisplayName);
				cnt++;
			}
			Console.WriteLine("F1 - install all");
			Console.WriteLine("Esc - exit");
			
		}
		
	
	}
	
}