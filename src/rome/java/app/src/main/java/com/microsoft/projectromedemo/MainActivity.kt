package com.microsoft.projectromedemo

import android.Manifest
import android.app.Activity
import android.app.Dialog
import android.content.Intent
import android.content.pm.PackageManager
import android.content.res.Configuration
import android.graphics.Bitmap
import android.net.Uri
import android.os.Bundle
import android.support.design.widget.FloatingActionButton
import android.support.design.widget.Snackbar
import android.support.v4.app.ActivityCompat
import android.support.v4.content.ContextCompat
import android.support.v7.app.AppCompatActivity
import android.support.v7.widget.Toolbar
import android.util.Log
import android.view.View
import android.view.Menu
import android.view.MenuItem
import android.webkit.WebChromeClient
import android.webkit.WebResourceRequest
import android.webkit.WebView
import android.webkit.WebViewClient

import com.microsoft.connecteddevices.IAuthCodeProvider
import com.microsoft.connecteddevices.IPlatformInitializationHandler
import com.microsoft.connecteddevices.Platform
import com.microsoft.connecteddevices.PlatformInitializationStatus
import java.util.*

class MainActivity : AppCompatActivity() {

    companion object {
        val APP_ID = "d92e47a8-cd0a-45f4-83b5-8001a14bd1fe";
        val REDIRECT_URI = "https://login.live.com/oauth20_desktop.srf";
        val TAG = "RomeSample"
    }

    lateinit var fab: FloatingActionButton;

    lateinit var authDialog: Dialog;
    lateinit var webView: WebView;

    lateinit var oauthUrl: String;
    lateinit var authCodeHandler: Platform.IAuthCodeHandler;

    private var permissionRequestCode: Int = -1;

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContentView(R.layout.activity_main)
        val toolbar = findViewById<View>(R.id.toolbar) as Toolbar
        setSupportActionBar(toolbar)

        fab = findViewById<View>(R.id.fab) as FloatingActionButton
        fab.hide()
        fab.setOnClickListener { view ->
            webView.loadUrl(oauthUrl)
            val webViewClient = object: WebViewClient() {
                var authComplete = false;
                override fun shouldOverrideUrlLoading(view: WebView?, request: WebResourceRequest?): Boolean {
                    return false
                }

                override fun onPageFinished(view: WebView?, url: String?) {
                    super.onPageFinished(view, url)

                    if (url == null) {
                        return
                    }

                    if (url.startsWith(REDIRECT_URI)) {
                        val uri = Uri.parse(url)
                        val code = uri.getQueryParameter("code")
                        val error = uri.getQueryParameter("error")

                        if (code != null && !authComplete) {
                            authComplete = true
                            authDialog.dismiss()
                            Log.i(TAG, "OAuth sign-in finished successfully")

                            authCodeHandler?.onAuthCodeFetched(code)
                        } else if (error != null) {
                            authComplete = true
                            Log.e(TAG, "OAuth sign-in failed with error ${error}")
                            setResult(Activity.RESULT_CANCELED, Intent())
                            authDialog.dismiss()
                        }
                    }
                }
            }

            webView.webViewClient = webViewClient
            authDialog.show()
            authDialog.setCancelable(true)

        }

        authDialog = Dialog(this).apply {
            setContentView(R.layout.auth_dialog);
        }
        webView = authDialog.findViewById<WebView>(R.id.webv).apply {
            webChromeClient = WebChromeClient()
            settings.javaScriptEnabled = true
            settings.domStorageEnabled = true
        }

        val random = Random()
        permissionRequestCode = random.nextInt(128)
        val permissionCheck = ContextCompat.checkSelfPermission(applicationContext, Manifest.permission.ACCESS_COARSE_LOCATION)
        if (permissionCheck == PackageManager.PERMISSION_DENIED) {
            ActivityCompat.requestPermissions(this, arrayOf(Manifest.permission.ACCESS_COARSE_LOCATION), permissionRequestCode)
        } else {
            initializePlatform()
        }
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)

        if (requestCode == permissionRequestCode) {
            initializePlatform()
            permissionRequestCode = -1
        }
    }

    private fun initializePlatform() {
        runOnUiThread {
            Platform.initialize(applicationContext,
                    object: IAuthCodeProvider {
                        override fun fetchAuthCodeAsync(url: String?, handler: Platform.IAuthCodeHandler?) {
                            if (url == null) {
                                fab.hide()
                            } else {
                                fab.show()
                            }

                            oauthUrl = url as String
                            authCodeHandler = handler as Platform.IAuthCodeHandler
                        }

                        override fun getClientId(): String {
                            return APP_ID
                        }
                    },
                    object: IPlatformInitializationHandler {
                        override fun onDone() {
                            Log.i(TAG, "Initialized platform sucessfully")
                            startActivity(Intent(this@MainActivity, DeviceListActivity::class.java))
                        }

                        override fun onError(status: PlatformInitializationStatus?) {
                            if (status == PlatformInitializationStatus.PLATFORM_FAILURE) {
                                Log.e(TAG, "Error initializing platform")
                            } else if (status == PlatformInitializationStatus.TOKEN_ERROR) {
                                Log.e(TAG, "Error refreshing tokens")
                            }
                        }
                    })
        }
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.menu_main, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        val id = item.itemId


        return if (id == R.id.action_settings) {
            true
        } else super.onOptionsItemSelected(item)

    }

}
