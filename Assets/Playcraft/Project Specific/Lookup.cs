using UnityEngine;

// Globally-accessible point of access to prefabs and scriptable object resources
[CreateAssetMenu(menuName = "Playcraft/Resource Library", fileName = "Resource Library")]
public class Lookup : ScriptableObject
{
	#region Singleton initialization
	public static Lookup instance { get; private set; } 

	[RuntimeInitializeOnLoadMethod]
	private static void Initialize() { instance = (Lookup)Resources.Load("Resource Library"); }
	#endregion


	#region Global Resource References
	
	public GameObject bulletPrefab;
	public ColorInfo colorInfo;
	public int colorCount => colorInfo.data.Length;
	
	#endregion


	#region Resource Lookups
	/*public ExampleLookup[] examples;

	public Example GetExample(ExampleType type)
    {
      	foreach (var item in examples)
      	  	if (item.type == type)
      	  	  	return item.value;
	
      return null;
    }*/
	#endregion
}

#region Resource Lookup Containers
/*[Serializable]
public struct ExampleLookup
{
	public ExampleType type;
	public Example value;
}*/
#endregion

#region Resource Identifier Enums
// public enum ExampleType { ExampleA, ExampleB, ExampleC }
#endregion