using System.Collections.Generic;

namespace BuzzIO
{
    public class BuzzHandsetFinder : IBuzzHandsetFinder
    {
        public IEnumerable<IBuzzHandsetDevice> FindHandsets()
        {
            return BuzzHandsetDevice.FindBuzzHandsets();
        }
    }
}
