using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inscryption_Server.Classes
{
	public struct CostType
	{
		public string Type {get;}
		public string Image {get;}
		public CostType(string type, string image)
		{
			Type = type;
			Image = image;
		}
	}
}
