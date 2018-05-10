package com.microsoft.projectromedemo.viewModels

import android.content.Context
import android.util.Log
import android.view.View

class ControllerActivityViewModel(private val context: Context, val device: Device) {
    fun launchApp(view: View) {
        device.launchApp { Log.i("RomeApp", it?.toString() ) }
    }

    fun connect(view: View) {
        device.connect()
    }

    fun sendMessage(view: View) {
        device.sendMessage()
    }
}
