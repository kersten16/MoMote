using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleFileBrowser;
using UnityEngine.UI;

public class FileSelector : MonoBehaviour
{
	// Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
	// Warning: FileBrowser can only show 1 dialog at a time
	public AppDataManager dataManager;
    public GameObject content;
	public GameObject menuItemPrefab;
	void Start()
	{

		// load from JSON

		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".obj" ));

		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		FileBrowser.SetDefaultFilter( ".obj" );

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

		setupMenu(dataManager.readFile());
		

		
	}

	public void setupMenu(List<AppData> menuList){
		foreach (var menuItem in menuList)
		{
			GameObject childObject = Instantiate(menuItemPrefab) as GameObject;
			childObject.GetComponent<modelLoader>().modelPath = menuItem.path;
			childObject.GetComponent<modelLoader>().modelName = menuItem.name;
			childObject.transform.SetParent(content.transform);
			childObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			//childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = Resources.Load<Sprite>(Application.persistentDataPath + "/" + menuItem.name.Split('.')[0] + ".png");
			
			childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = LoadNewSprite(Application.persistentDataPath + "/" + menuItem.name.Split('.')[0] + ".png");
		}
	}

	public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
	{

		// Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

		Texture2D SpriteTexture = LoadTexture(FilePath);
		Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

		return NewSprite;
	}

	public Texture2D LoadTexture(string FilePath)
	{

		// Load a PNG or JPG file from disk to a Texture2D
		// Returns null if load fails

		Texture2D Tex2D;
		byte[] FileData;

		if (File.Exists(FilePath))
		{
			FileData = File.ReadAllBytes(FilePath);
			Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
			if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
				return Tex2D;                 // If data = readable -> return texture
		}
		return null;                     // Return null if load failed
	}
	public void openFileSelector()
    {
		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Allow multiple selection: false
		// Initial path: "C:\", Initial filename: "Screenshot.png"
		// Title: "Save As", Submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

		// Show a select folder dialog 
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Allow multiple selection: false
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Select Folder", Submit button text: "Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		//						   () => { Debug.Log( "Canceled" ); },
		//						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

		// Coroutine example
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, false, null, null, "Load Files and Folders", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			
			dataManager.writeFile(FileBrowser.Result);
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for( int i = 0; i < FileBrowser.Result.Length; i++ ){
				Debug.Log( FileBrowser.Result[i] );
				// instantiate in content a new "model" with path and name as variable
				GameObject model = Instantiate(menuItemPrefab);
				model.GetComponent<modelLoader>().modelPath = FileBrowser.Result[i];
				model.GetComponent<modelLoader>().modelName = FileBrowserHelpers.GetFilename(FileBrowser.Result[i]);
				model.transform.SetParent(content.transform);
				model.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

				model.GetComponent<modelLoader>().loadModel();

				byte[] bytes = null;
				string path = Application.persistentDataPath + model.GetComponent<modelLoader>().modelName;
				System.IO.File.WriteAllBytes(path, bytes);
				// add to JSON
			}			
		}
	}
}