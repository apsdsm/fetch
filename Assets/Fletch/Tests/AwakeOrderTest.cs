using UnityEngine;
using System.Collections;

public class AwakeOrderTest : MonoBehaviour {


    void OnEnable ()
    {
        Debug.Log( "ENABLE: " + gameObject.name );

    }

    // on awake
    void Awake ()
    {
        Debug.Log( "AWAKE: " + gameObject.name );
        if ( gameObject.name == "GameObject1" )
        {
            Debug.Log( " GO1 here - looking for GO2..." );
            GameObject go = GameObject.Find( "GameObject2" );
            Debug.Log( "found:" + go.name );
        }

        if ( gameObject.name == "GameObject2" )
        {
            Debug.Log( " GO2 here - looking for GO1..." );
            GameObject go = GameObject.Find( "GameObject1" );
            Debug.Log( "found: " + go.name );
        }
    }


	// Use this for initialization
	void Start () {
        Debug.Log( "START: " + gameObject.name );
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log( "UPDATE: " + gameObject.name );
    }

    // on level load
    void OnLevelLoad ()
    {
        Debug.Log( "LEVELLOAD: " + gameObject.name );
    }
}
