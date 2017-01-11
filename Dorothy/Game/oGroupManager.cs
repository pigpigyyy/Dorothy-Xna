using System.Collections.Generic;

namespace Dorothy.Game
{
	public enum RelationShip
	{
		Friend = 0,
		Enemy,
		Neutral
	};

	public enum Group
	{
		Terrain = 0,
		Decoration,
		Destructable,
		JiangYou,
		PlayerOne,
		PlayerTwo,
		PlayerThree,
		PlayerFour,
		PlayerFive,
		PlayerSix,
		PlayerSeven,
		PlayerEight,
		PlayerNine
	};

	public class RelatedGroupInfo
	{
		public RelationShip RelationShip = RelationShip.Neutral;
		public bool ShouldContact = true;
		public object UserData;
	}

	public static class oGroupManager
	{
		private static Dictionary<uint, RelatedGroupInfo> _relationTable = new Dictionary<uint, RelatedGroupInfo>();

		public static void SetRelationShip(Group fromGroup, Group toGroup, RelationShip relation)
		{
			uint key = ((uint)fromGroup << 16 | (uint)toGroup);
			if (_relationTable.ContainsKey(key))
			{
				((RelatedGroupInfo)_relationTable[key]).RelationShip = relation;
			}
			else
			{
				RelatedGroupInfo groupInfo = new RelatedGroupInfo();
				groupInfo.RelationShip = relation;
				_relationTable.Add(key, groupInfo);
			}
		}
		public static RelationShip GetRelationShip(Group fromGroup, Group toGroup)
		{
			uint key = ((uint)fromGroup << 16 | (uint)toGroup);
			return ((RelatedGroupInfo)_relationTable[key]).RelationShip;
		}
		public static void SetShouldContact(Group groupOne, Group groupTwo, bool shouldContact)
		{
			uint key = ((uint)groupOne << 16 | (uint)groupTwo);
			if (_relationTable.ContainsKey(key))
			{
				((RelatedGroupInfo)_relationTable[key]).ShouldContact = shouldContact;
			}
			else
			{
				RelatedGroupInfo groupInfo = new RelatedGroupInfo();
				groupInfo.ShouldContact = shouldContact;
				_relationTable.Add(key, groupInfo);
				if (groupTwo != groupOne)
				{
					key = ((uint)groupTwo << 16 | (uint)groupOne);
					if (_relationTable.ContainsKey(key))
					{
						((RelatedGroupInfo)_relationTable[key]).ShouldContact = shouldContact;
					}
					else
					{
						_relationTable.Add(key, groupInfo);
					}
				}
			}
		}
		public static bool ShouldContact(Group groupOne, Group groupTwo)
		{
			uint key = ((uint)groupOne << 16 | (uint)groupTwo);
			return ((RelatedGroupInfo)_relationTable[key]).ShouldContact;
		}
		public static void SetUserData(Group fromGroup, Group toGroup, object userData)
		{
			uint key = ((uint)fromGroup << 16 | (uint)toGroup);
			if (_relationTable.ContainsKey(key))
			{
				((RelatedGroupInfo)_relationTable[key]).UserData = userData;
			}
			else
			{
				RelatedGroupInfo groupInfo = new RelatedGroupInfo();
				groupInfo.UserData = userData;
				_relationTable.Add(key, groupInfo);
			}
		}
		public static object GetUserData(Group fromGroup, Group toGroup)
		{
			uint key = ((uint)fromGroup << 16 | (uint)toGroup);
			return (RelatedGroupInfo)_relationTable[key].UserData;
		}
		public static void ClearAllInfo()
		{
			_relationTable.Clear();
		}
	}
}
