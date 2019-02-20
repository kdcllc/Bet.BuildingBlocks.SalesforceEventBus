using System;

namespace Bet.BuildingBlocks.SalesforceEventBus
{
    internal class SubscriptionInfo
    {
        public SubscriptionInfo(string name, int replayId, Type handlerType)
        {
            Name = name;
            ReplayId = replayId;
            HandlerType = handlerType;
        }
        public string Name { get; }

        public int ReplayId { get; set; }
        public object HandlerType { get; }

        #region Equals
        public bool Equals(SubscriptionInfo other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Name == Name &&  other.ReplayId == ReplayId && other.HandlerType == HandlerType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(SubscriptionInfo))
            {
                return false;
            }

            return Equals((SubscriptionInfo)obj);
        }

        public override int GetHashCode()
        {
            var hash = 15;
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + ReplayId.GetHashCode();
            hash = (hash * 7) + HandlerType.GetHashCode();

            return hash;
        }


        #endregion
    }
}
