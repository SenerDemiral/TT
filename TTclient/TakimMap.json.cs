using Starcounter;

namespace TTclient
{
	partial class TakimMap : Json
	{
		void Handle(Input.TakimMapOpened inp) {
			var parent = (TurnuvaTakimPage)this.Parent;
			parent.TakimMapOpened = false;
			parent.TakimMap = null;

		}
	}
}
