package com.microsoft.projectromedemo.viewModels

import com.microsoft.connecteddevices.RemoteSystem

class Device(private val remoteSystem: RemoteSystem) {
    val name: String;
    val type: String;
    val id: String;

    init {
        id = remoteSystem.id
        name = remoteSystem.displayName
        type = remoteSystem.kind.toString()
    }
}