using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum eStatus
    {
        SearchingUpdates,
        DownloadingUpdates,
        InstallingUpdates,
        FinishedUpdates,
        Error,
    }
}
