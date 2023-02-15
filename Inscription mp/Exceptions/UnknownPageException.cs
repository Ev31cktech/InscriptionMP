using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inscription_mp.Exceptions
{

	[Serializable]
	public class UnknownPageException : Exception
	{
		public UnknownPageException() : base("Unknown Page") { }
		public UnknownPageException(string message) : base(message) { }
		public UnknownPageException(string message, Exception inner) : base(message, inner) { }
		protected UnknownPageException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
