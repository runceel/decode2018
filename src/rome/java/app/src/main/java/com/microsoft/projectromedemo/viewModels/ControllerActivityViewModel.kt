package com.microsoft.projectromedemo.viewModels

import android.content.Context
import android.databinding.ObservableField
import android.util.Log
import android.view.View

class ControllerActivityViewModel(private val context: Context, val device: Device) {

    val input = ObservableField<String>("");

    fun launchApp(view: View) {
        device.launchApp(input.get() as String) { Log.i("RomeApp", it?.toString() ) }
    }

    fun connect(view: View) {
        device.connect { x ->
            input.set(x)
        }
    }

    fun sendMessage(view: View) {
        device.sendMessage(input.get() as String)
    }
}
