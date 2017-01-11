using Box2D;

namespace Dorothy.Game
{
	public class ContactListener : IContactListener
	{
		public void BeginContact(Contact contact)
		{
			Fixture fixtureA = contact.GetFixtureA();
			Fixture fixtureB = contact.GetFixtureB();
			Unit unitA = (Unit)fixtureA.GetBody().GetUserData();
			Unit unitB = (Unit)fixtureB.GetBody().GetUserData();
			bool bContact = oGroupManager.ShouldContact(unitA.Group, unitB.Group);
			if (bContact)
			{
				if (fixtureA.IsSensor())
				{
					Sensor sensor = (Sensor)fixtureA.GetUserData();
					if (sensor.Enable && !fixtureB.IsSensor())
					{
						sensor.Add(unitB);
					}
				}
				if (fixtureB.IsSensor())
				{
					Sensor sensor = (Sensor)fixtureB.GetUserData();
					if (sensor.Enable && !fixtureA.IsSensor())
					{
						sensor.Add(unitA);
					}
				}
			}
		}
		public void EndContact(Contact contact)
		{
			Fixture fixtureA = contact.GetFixtureA();
			Fixture fixtureB = contact.GetFixtureB();
			if (fixtureA.IsSensor())
			{
				Sensor sensor = (Sensor)fixtureA.GetUserData();
				Unit unitB = (Unit)fixtureB.GetBody().GetUserData();
				if (sensor.Enable)
				{
					sensor.Remove(unitB);
				}
			}
			if (fixtureB.IsSensor())
			{
				Sensor sensor = (Sensor)fixtureB.GetUserData();
				Unit unitA = (Unit)fixtureA.GetBody().GetUserData();
				if (sensor.Enable)
				{
					sensor.Remove(unitA);
				}
			}
		}
		public void PreSolve(Contact contact, ref Manifold oldManifold)
		{
			Unit unitA = (Unit)contact.GetFixtureA().GetBody().GetUserData();
			Unit unitB = (Unit)contact.GetFixtureB().GetBody().GetUserData();
			if (!(unitA.EnableCollision && unitB.EnableCollision
				&& oGroupManager.ShouldContact(unitA.Group, unitB.Group)))
			{
				contact.SetEnabled(false);
			}
		}
		public void PostSolve(Contact contact, ref ContactImpulse impulse)
		{ }
	}
}
