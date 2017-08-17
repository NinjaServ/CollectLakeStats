namespace LakeStats
{
    class FeedResult
    {
        public double mean { get; set; }
        public double median { get; set; }
        public int count { get; set; }

        //        public override string ToString()
        //        {
        //#if (DEBUG)

        //            // String Interopting 
        //            return $"{{Count: {count}, Mean: {mean}, Median: {median}}}";

        //            // {date:MM/dd/yy}
        //#else
        //            return "{Count: " + count + 
        //                ",Mean:" + mean + 
        //                ",Median:" + median + "}";
        //#endif
        //        }


        public override string ToString() => $"{{Count: {count}, Mean: {mean}, Median: {median}}}";
    }
}

//Extensions
//Power productivity tools
//web essentials
//code maid
//Viasfora

//cntrl+Q -> Format
// Cntrl+K, Cntrl+D

