namespace Core.Input
{
    public struct SwipeProgress
    {
        public Direction Direction { get; }
        public float Progress { get; }

        public SwipeProgress(Direction direction, float progress)
        {
            Direction = direction;
            Progress = progress;
        }
    }
}