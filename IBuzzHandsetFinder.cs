using System.Collections.Generic;

namespace BuzzIO
{
    public interface IBuzzHandsetFinder
    {
        IEnumerable<IBuzzHandsetDevice> FindHandsets();
    }
}
