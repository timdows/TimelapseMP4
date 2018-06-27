import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-mp4-list',
	templateUrl: './mp4-list.component.pug',
	styleUrls: ['./mp4-list.component.scss']
})
export class Mp4ListComponent implements OnInit {
	public mp4Details: MP4Details[];

	constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
		http.get<MP4Details[]>(baseUrl + 'api/MP4List/GetList').subscribe(result => {
		  this.mp4Details = result;
		}, error => console.error(error));
	  }

	ngOnInit() {
	}

}

interface MP4Details {
	name: string;
	date: Date;
}