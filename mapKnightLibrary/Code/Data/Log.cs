using System;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;

namespace mapKnightLibrary
{
	public class CrossLog{
		
		public static void Log(string project, string tag, string message, MessageType type, Exception errorexception = null){
			// loggt eine Nachricht mit verstellbaren Variablen (meistens nicht von der PCL Bibliothek,
			// weswegen auch ein 'project' angegeben werden muss)
			switch (type) {
			case MessageType.Debug:
				DependencyService.Get<ILog> ().Debug (project, tag, message);
				break;
			case MessageType.Error:
				if (errorexception != null) {
					DependencyService.Get<ILog> ().Error (project, tag, errorexception);
				} else {
					Log ("PortableLibrary", "CrossLog", tag + " @ " + project + "used the ErrorMessageType without having an error", MessageType.WTF, new ArgumentException ("no error given"));
				}
				break;
			case MessageType.Info:
				DependencyService.Get<ILog> ().Info (project, tag, message);
				break;
			case MessageType.Warn:
				DependencyService.Get<ILog> ().Warn (project, tag, message);
				break;
			case MessageType.WTF:
				if (errorexception != null) {
					DependencyService.Get<ILog> ().WTF (project, tag, message, errorexception);
				} else {
					Log ("PortableLibrary", "CrossLog", tag + " @ " + project + "used the ErrorMessageType without having an error", MessageType.WTF, new ArgumentException ("no error given"));
				}
				break;
			}
		}

		public static void Log(object sender, string message, MessageType type, Exception errorexception = null){
			// loggt eine Nachricht mit einem auf dem sender basierenden tag (nur von PCL Bibliothek internen Klassen möglich)
			if (tagRegister.ContainsKey (sender.GetType ())) {
				switch (type) {
				case MessageType.Debug:
					DependencyService.Get<ILog> ().Debug ("PortableLibrary", tagRegister [sender.GetType ()], message);
					break;
				case MessageType.Error:
					if (errorexception != null) {
						DependencyService.Get<ILog> ().Error ("PortableLibrary", tagRegister [sender.GetType ()], errorexception);
					} else {
						Log ("PortableLibrary", "CrossLog", tagRegister [sender.GetType ()] + " @ " + "PortableLibrary" + "used the ErrorMessageType without having an error", MessageType.WTF, new ArgumentException ("no error given"));
					}
					break;
				case MessageType.Info:
					DependencyService.Get<ILog> ().Info ("PortableLibrary", tagRegister [sender.GetType ()], message);
					break;
				case MessageType.Warn:
					DependencyService.Get<ILog> ().Warn ("PortableLibrary", tagRegister [sender.GetType ()], message);
					break;
				case MessageType.WTF:
					if (errorexception != null) {
						DependencyService.Get<ILog> ().WTF ("PortableLibrary", tagRegister [sender.GetType ()], message, errorexception);
					} else {
						Log ("PortableLibrary", "CrossLog", tagRegister [sender.GetType ()] + " @ " + "PortableLibrary" + "used the ErrorMessageType without having an error", MessageType.WTF, new ArgumentException ("no error given"));
					}
					break;
				}
			} else {
				throw new ArgumentException (sender.ToString() + " tried to log a message with its object related type without being registered");
			}
		}

		public static Dictionary<Type, string> tagRegister = new Dictionary<Type, string> () {
			{ typeof(GameScene), "GameScene" },
			{ typeof(MergedLayer),"MergedLayer" },
			{ typeof(Inventory.GameInventory),"Inventory" },
			{ typeof(Chest),"StandartChest" },
			{ typeof(Container),"Container" },
			{ typeof(JumpPad),"StandartJumpPad" },
			{ typeof(Platform),"StandartPlatform" },
			{ typeof(RoboBob),"Character/RoboBob" },
			{ typeof(TMXLayerDataLoader),"TXMLayerDataLoader" },
			{ typeof(CollusionSensor),"CollusionSensor" },
			{ typeof(JumpManager),"JumpManager" },
			{ typeof(PhysicsHandler),"PhysicsHandler" },
			{ typeof(AppExitNotifier),"ApplicationExitNotifier" },
			{ typeof(XMLElemental),"XMLElemental" }
		};
	}

	public enum MessageType{
		Debug = 0,
		Info = 1,
		Warn = 2,
		Error = 3,
		WTF = 1337
	}

	public interface ILog{
		void Debug (string project, string tag, string message); //Sollte benutzt werden um CodeInformationen zu geben
		void Error (string project, string tag, Exception ex);
		void Info (string project, string tag, string message); //Sollte benutzt werden um VariablenWerte zu geben
		void Warn (string project, string tag, string message); //Sollte benutzt werden um auf kritische Situationen hinzuweisen
		void WTF (string project, string tag, string message, Exception ex); //YOLO
	}
}