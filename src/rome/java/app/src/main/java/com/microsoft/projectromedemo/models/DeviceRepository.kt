package com.microsoft.projectromedemo.models

import android.databinding.ObservableArrayList
import com.microsoft.projectromedemo.viewModels.Device

object DeviceRepository {
    val devices = ObservableArrayList<Device>()

    fun getDeviceById(id: String): Device {
        return devices.first { it.id == id }
    }

    fun add(device: Device) {
        devices.add(device)
    }
}