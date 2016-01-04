﻿using UnityEngine;
using UnityEditor;
using System.IO;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCServiceTest" )]
    public class it_adds_existing_children_to_directory_upon_creation : MonoBehaviour
    {

        GameObject ioc_instance;
        IOCService ioc_service;

        // setup
        void Awake ()
        {
            // create a temp directory to store a prefab
            Directory.CreateDirectory( @"Assets/__FletchTests" );

            // create a new game object
            GameObject ioc_prefab = new FlexoGameObject().WithParent( gameObject ).With<IOCService>().WithChild( "Foo" ).Where( "Foo" ).Has<FletchTestService>().AsPrefab( @"Assets/__FletchTests/IOC_with_children.prefab" );

            // add the prefab to the game scene
            ioc_instance = PrefabUtility.InstantiatePrefab( ioc_prefab ) as GameObject;
            ioc_service = ioc_instance.GetComponent<IOCService>();
        }

        // test
        void Update ()
        {
            int serviceCount = ioc_service.RegisteredServices().Length;

            IntegrationTest.Assert( serviceCount == 1, "should be 1 resolvable service but found: " + serviceCount.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_instance );
            Directory.Delete( @"Assets/__FletchTests", true );
            File.Delete( @"Assets/__FletchTests.meta" );
        }
    }
}