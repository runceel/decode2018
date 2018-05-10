package com.microsoft.projectromedemo.viewModels

import android.databinding.Observable
import android.databinding.ObservableBoolean
import android.net.Uri
import android.os.Bundle
import android.util.Log
import com.microsoft.connecteddevices.*
import java.util.*

class Device(private val remoteSystem: RemoteSystem) {
    val name: String;
    val type: String;
    val id: String;

    val isLaunched = ObservableBoolean(false)
    val isConnected = ObservableBoolean(false)

    private lateinit var connection: AppServiceConnection;

    init {
        id = remoteSystem.id
        name = remoteSystem.displayName
        type = remoteSystem.kind.toString()
    }

    fun launchApp(callback: (RemoteLaunchUriStatus?) -> Unit) {
        RemoteLauncher.LaunchUriAsync(RemoteSystemConnectionRequest(remoteSystem),
                Uri.parse("decode18://"),
                object: IRemoteLauncherListener {
                    override fun onCompleted(status: RemoteLaunchUriStatus?) {
                        callback(status)
                        isLaunched.set(status == RemoteLaunchUriStatus.SUCCESS)
                    }
                })
    }

    fun connect() {
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
                { req -> })
        connection.openRemoteAsync()
    }

    fun sendMessage() {
        if (!isConnected.get()) {
            return
        }

        val message = "Message from Android ${Date()}"
        val messageBundle = Bundle().apply {
            putString("Request", message)
        }
        connection.sendMessageAsync(messageBundle, { res: AppServiceResponse ->

        })
    }
}