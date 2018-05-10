package com.microsoft.projectromedemo

import android.databinding.DataBindingUtil
import android.os.Bundle
import android.support.design.widget.Snackbar
import android.support.v7.app.AppCompatActivity
import com.microsoft.projectromedemo.databinding.ActivityControllerBinding
import com.microsoft.projectromedemo.models.DeviceRepository
import com.microsoft.projectromedemo.viewModels.ControllerActivityViewModel

import kotlinx.android.synthetic.main.activity_controller.*

class ControllerActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val id = intent.getStringExtra("id")
        val binding = DataBindingUtil.setContentView<ActivityControllerBinding>(this, R.layout.activity_controller).let {
            it.viewModel = ControllerActivityViewModel(this, DeviceRepository.getDeviceById(id))
        }
        setSupportActionBar(toolbar)
    }

}
