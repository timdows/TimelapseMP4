import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { Mp4ListComponent } from './mp4-list/mp4-list.component';
import { Mp4PlayerComponent } from './mp4-player/mp4-player.component';
import { Hour1400ImagesComponent } from './hour1400-images/hour1400-images.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    Mp4ListComponent,
    Mp4PlayerComponent,
    Hour1400ImagesComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
		{ path: 'hour1400', component: Hour1400ImagesComponent },
		{ path: '', component: HomeComponent, pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
