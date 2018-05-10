package com.microsoft.projectromedemo

import android.databinding.DataBindingUtil
import android.os.Bundle
import android.support.design.widget.Snackbar
import android.support.v7.app.AppCompatActivity
import android.util.Log
import android.view.View
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.ListView
import com.microsoft.connecteddevices.ConnectedDevicesException
import com.microsoft.connecteddevices.IRemoteSystemDiscoveryListener
import com.microsoft.connecteddevices.RemoteSystem
import com.microsoft.connecteddevices.RemoteSystemDiscovery
import com.microsoft.projectromedemo.databinding.ActivityDeviceListBinding
import com.microsoft.projectromedemo.viewModels.Device
import com.microsoft.projectromedemo.viewModels.DeviceListViewModel

import kotlinx.android.synthetic.main.activity_device_list.*

class DeviceListActivity : AppCompatActivity() {

    private var discovery: RemoteSystemDiscovery? = null;
    private val viewModel = DeviceListViewModel();

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val binding = DataBindingUtil.setContentView<ActivityDeviceListBinding>(this, R.layout.activity_device_list).let {
            it.viewModel = viewModel
        }
        setSupportActionBar(toolbar)

        initData()
    }

    fun deviceSelected(parent: AdapterView<*>, view: View, position: Int, id: Long) {
        val device = parent.selectedItem as Device
        Log.i("MainActivity", device.name);
    }

    private fun initData() {
        val b = RemoteSystemDiscovery.Builder().let {
            it.setListener(object: IRemoteSystemDiscoveryListener {
                override fun onComplete() {
                }

                override fun onRemoteSystemRemoved(p0: String?) {
                }

                override fun onRemoteSystemUpdated(p0: RemoteSystem?) {
                }

                override fun onRemoteSystemAdded(remoteSystem: RemoteSystem?) {
                    if (remoteSystem == null) {
                        return
                    }

                    runOnUiThread({
                        viewModel.deviceList.add(Device(remoteSystem))
                    })
                }
            })
        }
        startDiscover(b)
    }

    private fun startDiscover(builder: RemoteSystemDiscovery.Builder) {
        try {
            discovery?.stop()
        } catch (ex: ConnectedDevicesException) {
            ex.printStackTrace()
        }
        discovery = builder.result
        try {
            discovery?.start()
        } catch (ex: ConnectedDevicesException) {
            ex.printStackTrace()
        }
    }

}
