namespace MusicRater.Model
{
    /// <summary>
    /// Information about a contest
    /// </summary>
    public class ContestInfo
    {
        /// <summary>
        /// A friendly name for this contest
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// URL to download tracklist
        /// </summary>
        public string TrackListUrl { get; set; }
        /// <summary>
        /// File name to use in iso store
        /// </summary>
        public string IsoStoreFileName { get; set; }
    }
}