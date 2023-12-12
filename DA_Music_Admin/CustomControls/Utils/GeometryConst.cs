using System.Windows.Media;

namespace CustomControls.Utils
{
    public static class GeometryConst
    {
        public static Geometry IconCheck;
        public static Geometry IconError;
        public static Geometry IconInfor;
        public static Geometry IconWarning;
        
        public static void Register()
        {
            IconCheck = Geometry.Parse("M9,20.42L2.79,14.21L5.62,11.38L9,14.77L18.88,4.88L21.71,7.71L9,20.42Z");
            IconError = Geometry.Parse("M13,13H11V7H13M13,17H11V15H13M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z");
            IconInfor = Geometry.Parse("M13,9H11V7H13M13,17H11V11H13M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z");
            IconWarning = Geometry.Parse("M13,13H11V7H13M13,17H11V15H13M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z");
        }
    }
}
