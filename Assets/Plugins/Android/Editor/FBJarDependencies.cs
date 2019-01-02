using Google.JarResolver;
using UnityEditor;

[InitializeOnLoad]
public static class FBJarDependencies
{
    private static readonly string PluginName = "FBApiAccess";
    public static PlayServicesSupport svcSupport;

    static FBJarDependencies()
    {

        svcSupport = PlayServicesSupport.CreateInstance(PluginName, EditorPrefs.GetString("AndroidSdkRoot"), "ProjectSettings");
        svcSupport.DependOn("com.google.firebase", "firebase-database", "11.8.0");
        svcSupport.DependOn("com.android.support", "support-v13", "23.4.0");
        svcSupport.DependOn("com.google.gms", "google-services", "3.1.1");

    }
}