using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChampionsOfForest.Network
{
	public class CommandStream : MemoryStream
	{
		BinaryWriter writer;
		public CommandStream(Commands.CommandType commandType) : base()
		{
			writer = new BinaryWriter(this);
			writer.Write((int)commandType);
		}
		public override void Close()
		{
			writer.Close();
			base.Close();
		}
		public BinaryWriter Writer => writer;



		public void Send(NetworkManager.Target targets)
		{
			Network.NetworkManager.SendCommand(this, targets);
			Close();
		}

		public override string ToString()
		{
			Position = 0;
			using (StreamReader reader = new StreamReader(this))
			{
				return reader.ReadToEnd();
			}
		}

	}
}
