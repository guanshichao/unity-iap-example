//参考资料：https://www.jianshu.com/p/6bed4fc32aeb
//参考资料：https://csharp.hotexamples.com/examples/UnityEditor.iOS.Xcode/PBXProject/SetCompileFlagsForFile/php-pbxproject-setcompileflagsforfile-method-examples.html
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using System.IO;

public class EditXcodeProject
{
    //该属性是在build完成后，被调用的callback
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        Debug.LogFormat("Begin ChangeXcodePlist,buildTarget:{0},pathToBuiltProject:{1}",buildTarget,pathToBuiltProject);
        if (buildTarget != BuildTarget.iOS)
            return;
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        if (!File.Exists(projectPath))
        {
            Debug.LogErrorFormat("no file:{0}",projectPath);
            return;
        }
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        // 添加framwrok
        pbxProject.AddFrameworkToProject(targetGuid, "StoreKit.framework", false);


        SetCompileFlagsForFile(pbxProject, "Libraries/Plugins/iOS/IAPTransactionObserver.m", "-fno-objc-arc");
        SetCompileFlagsForFile(pbxProject, "Libraries/Plugins/iOS/SecureData.m", "-fno-objc-arc");
        // 应用修改
        File.WriteAllText(projectPath, pbxProject.WriteToString());
        Debug.Log("end ChangeXcodePlist");
    }
    private static void SetCompileFlagsForFile(PBXProject pbxProject,string filepath,string compileFlags)
    {
        string target = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName());
        string fileGuid = pbxProject.FindFileGuidByProjectPath(filepath);
        //pbxProject.buildphase
        List<string> list = new List<string>();
        list.Add(compileFlags);
        pbxProject.SetCompileFlagsForFile(target, fileGuid, list);
    }
    //[MenuItem("tools/t1")]
    //private static void D1()
    //{
    //    var paths = AssetDatabase.GetAllAssetPaths();
    //    foreach (var item in paths)
    //    {
    //        if (item.Contains("IAPTransactionObserver.m") || item.Contains("SecureData.m"))
    //        {
    //            Debug.Log(item);
    //        }
    //    }
    //}
}
