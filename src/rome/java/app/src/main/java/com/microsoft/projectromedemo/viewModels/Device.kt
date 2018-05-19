package com.microsoft.projectromedemo.viewModels

import android.databinding.Observable
import android.databinding.ObservableBoolean
import android.net.Uri
import android.os.Bundle
import android.util.Log
import com.microsoft.connecteddevices.*
import java.net.URI
import java.net.URLEncoder
import java.util.*

class Device(private val remoteSystem: RemoteSystem) {
    val name: String;
    val type: String;
    val id: String;
    val isAvailableByProximity: String;

    val isLaunched = ObservableBoolean(false)
    val isConnected = ObservableBoolean(false)

    private lateinit var connection: AppServiceConnection;

    init {
        id = remoteSystem.id
        name = remoteSystem.displayName
        type = remoteSystem.kind.toString()
        isAvailableByProximity = remoteSystem.isAvailableByProximity.toString()
    }

    fun launchApp(text: String, callback: (RemoteLaunchUriStatus?) -> Unit) {
        RemoteLauncher.LaunchUriAsync(RemoteSystemConnectionRequest(remoteSystem),
                Uri.parse("decode18:?text=${URLEncoder.encode(text, "utf-8")}"),
                object: IRemoteLauncherListener {
                    override fun onCompleted(status: RemoteLaunchUriStatus?) {
                        callback(status)
                        isLaunched.set(status == RemoteLaunchUriStatus.SUCCESS)
                    }
                })
    }

    fun connect(callback: (String) -> Unit) {
        connection = AppServiceConnection("RomeAppService",
                "2d7f326b-d78c-494e-9649-bfc98e52a8b3_5ppbtxp1sbcde",
                RemoteSystemConnectionRequest(remoteSystem),
                object: IAppServiceConnectionListener {
                    override fun onSuccess() {
                        Log.i("RomeApp", "Connection success.")
                        isConnected.set(true)
                    }
                    override fun onClosed(status: AppServiceConnectionClosedStatus?) {
                        Log.i("RomeApp", "Connection closed: ${status}.")
                        isConnected.set(false)
                    }
                    override fun onError(status: AppServiceConnectionStatus?) {
                        Log.i("RomeApp", "Connection failed: ${status}.")
                        isConnected.set(false)
                    }
                },
                { req ->
                    Log.i("RomeApp", "Received ${req.message.getString("Request")}")
                    callback(req.message.getString("Request"))
                })
        connection.openRemoteAsync()
    }

    fun sendMessage(text: String) {
        if (!isConnected.get()) {
            return
        }

        val message = text
        val messageBundle = Bundle().apply {
            putString("Request", message)
        }
        connection.sendMessageAsync(messageBundle, { res: AppServiceResponse ->

        })
    }
}