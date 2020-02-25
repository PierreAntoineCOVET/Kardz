import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoincheComponent } from './coinche.component';

describe('CoincheComponent', () => {
  let component: CoincheComponent;
  let fixture: ComponentFixture<CoincheComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoincheComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoincheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
