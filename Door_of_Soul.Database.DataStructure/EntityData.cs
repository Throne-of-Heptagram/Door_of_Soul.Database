using Door_of_Soul.Core;

namespace Door_of_Soul.Database.DataStructure
{
    public struct EntityData
    {
        public int entityId;
        public int existedSceneId;
        public SimpleVector3 position;
        public SimpleVector3 eulerAngles;
        public SimpleVector3 scale;
        public float mass;
        public float drag;
        public float angularDrag;
        public bool useGravity;
    }
}
