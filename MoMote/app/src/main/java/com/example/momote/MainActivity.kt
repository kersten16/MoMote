package com.example.momote

import android.os.Bundle
import android.view.*
import androidx.activity.ComponentActivity
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.compose.setContent
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalConfiguration
import androidx.compose.ui.unit.dp
import coil.compose.rememberAsyncImagePainter
import com.example.momote.ui.theme.MoMoteTheme
import com.google.android.filament.Skybox
import com.google.android.filament.utils.*
import java.nio.ByteBuffer

data class Model(val name: String, val path: String)


class MainActivity : ComponentActivity() {


    companion object {
        // Load the library for the utility layer, which in turn loads gltfio and the Filament core.
        init { Utils.init() }
        private const val TAG = "gltf-viewer"
    }

    private lateinit var surfaceView: SurfaceView
    private lateinit var choreographer: Choreographer
    private lateinit var modelViewer: ModelViewer

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        surfaceView = SurfaceView(this).apply { setContentView(this) }
        choreographer = Choreographer.getInstance()
        modelViewer = ModelViewer(surfaceView)
        surfaceView.setOnTouchListener(modelViewer)
        //loadGlb("DamagedHelmet")
        loadGltf("BusterDrone")
        loadEnvironment("venetian_crossroads_2k")
        modelViewer.scene.skybox = Skybox.Builder().build(modelViewer.engine)
        
        /*
        setContent {
            MoMoteTheme {
                // A surface container using the 'background' color from the theme
                Surface(modifier = Modifier.fillMaxSize(), color = MaterialTheme.colors.background) {
                    MenuList()
                }
            }
        }
        */
    }

    private fun loadGlb(name: String) {
        val buffer = readAsset("models/${name}.glb")
        modelViewer.loadModelGlb(buffer)
        modelViewer.transformToUnitCube()
    }

    fun loadGltf(name: String) {
        /* TODO change from read asset to read from a file on the phone */
        val buffer = readAsset("models/${name}.gltf")
        modelViewer.loadModelGltf(buffer) { uri -> readAsset("models/$uri") }
        modelViewer.transformToUnitCube()
        /* TODO switch to the model viewer */
    }

    private fun loadEnvironment(ibl: String) {
        // Create the indirect light source and add it to the scene.
        var buffer = readAsset("envs/$ibl/${ibl}_ibl.ktx")
        KTX1Loader.createIndirectLight(modelViewer.engine, buffer).apply {
            intensity = 50_000f
            modelViewer.scene.indirectLight = this
        }

        // Create the sky box and add it to the scene.
        buffer = readAsset("envs/$ibl/${ibl}_skybox.ktx")
        KTX1Loader.createSkybox(modelViewer.engine, buffer).apply {
            modelViewer.scene.skybox = this
        }
    }
    private fun readAsset(assetName: String): ByteBuffer {
        val input = assets.open(assetName)
        val bytes = ByteArray(input.available())
        input.read(bytes)
        return ByteBuffer.wrap(bytes)
    }

    private val frameCallback = object : Choreographer.FrameCallback {
        override fun doFrame(currentTime: Long) {
            choreographer.postFrameCallback(this)
            modelViewer.render(currentTime)
        }
    }

    override fun onResume() {
        super.onResume()
        choreographer.postFrameCallback(frameCallback)
    }

    override fun onPause() {
        super.onPause()
        choreographer.removeFrameCallback(frameCallback)
    }

    override fun onDestroy() {
        super.onDestroy()
        choreographer.removeFrameCallback(frameCallback)
    }


    val hPad = 16.dp
    val vPad = 10.dp

    @Composable
    fun MenuItem(model: Model) {
        val (height, width) = LocalConfiguration.current.run { screenHeightDp.dp to screenWidthDp.dp }

        Button(shape = MaterialTheme.shapes.medium, onClick = { /*TODO open 3d model viewer https://github.com/google/filament*/ loadGltf("BusterDrone")}, colors = ButtonDefaults.buttonColors(backgroundColor = MaterialTheme.colors.background)) {
            Column() {
                // a thumbnail of what we want
                Image(
                    painter = rememberAsyncImagePainter(model = model.path),
                    contentDescription = "Model Preview",
                    contentScale = ContentScale.Fit,
                    modifier = Modifier.size(width - hPad*2),
                )
                // a text with the model name
                Text(
                    text = model.name,
                    modifier = Modifier.padding(all = 4.dp),
                    style = MaterialTheme.typography.body2
                )
            }
        }
    }

    @Composable
    fun MenuList(){
        var modelList = remember {
            /* State is to say that when it changes, composables that use it will update
            * but we put it in a remember so that it remembers its values if not, it'll be reinitialized */
            mutableStateListOf<Model>(Model("test","a")/*,
        Model("test2", "path"),
        Model("test3", "path")*/)
        }

        // File viewer open and add to list         https://stackoverflow.com/questions/68768236/how-do-i-start-file-chooser-using-intent-in-compose
        val pickPictureLauncher = rememberLauncherForActivityResult(
            ActivityResultContracts.GetContent()
        ) { imageUri ->
            if (imageUri != null) {
                println(imageUri)
                /* TODO change from uri to path */
                // adding to list
                modelList.add(Model(imageUri.lastPathSegment.toString(), imageUri.toString()))

                // Update the state with the Uri
            }
        }

        LazyColumn(verticalArrangement = Arrangement.spacedBy(vPad/2), contentPadding = PaddingValues(hPad, vPad)){
            // adds an item for each element in modelList
            items(modelList){ model ->
                MenuItem(model = model)
            }

            // the item for the plus button
            item{
                Button(modifier = Modifier.fillMaxSize(),
                    onClick = {
                        pickPictureLauncher.launch("image/*")
                    })
                {
                    Text(text = "+")
                }
            }
        }


    }
}
