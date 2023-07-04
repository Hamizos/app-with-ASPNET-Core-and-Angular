import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeberDetailComponent } from './meber-detail.component';

describe('MeberDetailComponent', () => {
  let component: MeberDetailComponent;
  let fixture: ComponentFixture<MeberDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MeberDetailComponent]
    });
    fixture = TestBed.createComponent(MeberDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
