using System;
using System.IO;
using System.Runtime.InteropServices;

using UnityEngine.Windows.Speech;

namespace ChampionsOfForest.Network
{
	public class COTFCommand<ParamT> where ParamT : struct
	{
		public int commandIndex = 0;
		private static COTFCommand<ParamT> instance;

		public delegate void ReceivedDelegate(ParamT p);

		private ReceivedDelegate receivedDelegate;

		public static void Initialize(ReceivedDelegate receivedDelegate)
		{
			if (instance == null)
			{
				instance = new COTFCommand<ParamT>();
				instance.commandIndex = CommandReader.curr_cmd_index++;
				CommandReader.callbacks.Add(instance.commandIndex, COTFCommand<ParamT>.Received);
				instance.receivedDelegate = receivedDelegate;

				Type type = typeof(COTFCommand<ParamT>);
				ModAPI.Log.Write($"command {instance.commandIndex} registered: {type.Name}");
			}
		}

	

		public static void Send(NetworkManager.Target target, in ParamT param)
		{
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(instance.commandIndex);
					w.Write(Utils.GetBytesFromObject(param));
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), target);
				answerStream.Close();
			}
		}
		public static void Send(BoltConnection target_connection, ParamT param)
		{
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(instance.commandIndex);
					w.Write(Utils.GetBytesFromObject(param));
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), target_connection);
				answerStream.Close();
			}
		}
		public static void Received(BinaryReader r)
		{
			ParamT p = Utils.GetObjectFromBytes<ParamT>(r);
			instance.receivedDelegate.Invoke(p);
			r.Close();
		}
	}
}
