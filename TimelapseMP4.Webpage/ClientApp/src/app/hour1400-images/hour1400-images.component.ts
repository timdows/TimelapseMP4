import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Hour1400File } from '../../api/model/hour1400File';
import { Hour1400Service } from '../../api';

@Component({
	selector: 'app-hour1400-images',
	templateUrl: './hour1400-images.component.pug',
	styleUrls: ['./hour1400-images.component.scss']
})
export class Hour1400ImagesComponent implements OnInit {
	
	private baseUrl: string;
	public hour1400Files: Hour1400File[];
	public flyoutImage: Hour1400File;

	constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
		this.baseUrl = baseUrl;
		let hour1400Service = new Hour1400Service(http, this.baseUrl.replace(/\/+$/, ""), null);
		hour1400Service.apiHour1400GetThumbnailListGet().subscribe(result => {
			this.hour1400Files = result;
		});
	}

	ngOnInit() {
	}

	getImagePath(hour1400File: Hour1400File) {
		return `${this.baseUrl}api/Hour1400/GetImage?fileName=${hour1400File.fileName}`;
	}

	showImage(hour1400File: Hour1400File) {
		var fileName = hour1400File.fileName.replace("_thumb", "");
		return `${this.baseUrl}api/Hour1400/GetImage?fileName=${fileName}`;
	}

	clickImage(hour1400File: Hour1400File) {
		this.flyoutImage = hour1400File;
	}

	removeFlyout() {
		this.flyoutImage = null;
	}

}