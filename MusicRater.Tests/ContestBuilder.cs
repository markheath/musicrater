using MusicRater.Model;

namespace MusicRater.Tests
{
    class ContestBuilder
    {
        public string FileName = "blah.xml";
        public string LoadUrl = "http://ignored.com/blah.xml";

        public Contest Build()
        {
            var contest = new Contest(FileName, LoadUrl);
            return contest;
        }

        public Contest BuildWithTracks(int trackCount)
        {
            var contest = Build();
            var trackBuilder = new TrackBuilder();
            for (int n = 0; n < trackCount; n++)
            {
                contest.Tracks.Add(trackBuilder.BuildWithTitle("Track " + (n+1).ToString()));
            }
            return contest;
        }
    }
}
