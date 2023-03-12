package com.example.momote

import android.content.res.Resources
import android.os.Bundle
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
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.rememberAsyncImagePainter
import coil.compose.rememberImagePainter
import com.example.momote.ui.theme.MoMoteTheme

data class Model(val name: String, val path: String)


class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MoMoteTheme {
                // A surface container using the 'background' color from the theme
                Surface(modifier = Modifier.fillMaxSize(), color = MaterialTheme.colors.background) {
                    MenuList()
                }
            }
        }
    }
}

val hPad = 16.dp
val vPad = 10.dp

@Composable
fun MenuItem(model: Model) {
    val (height, width) = LocalConfiguration.current.run { screenHeightDp.dp to screenWidthDp.dp }

    Button(shape = MaterialTheme.shapes.medium, onClick = { /*TODO open 3d model viewer*/ }, colors = ButtonDefaults.buttonColors(backgroundColor = MaterialTheme.colors.background)) {
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
            /* TODO add to model list */
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