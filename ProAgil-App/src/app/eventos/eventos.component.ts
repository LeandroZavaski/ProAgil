import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventosFiltrados: Evento  [];
  eventos: Evento[];
  imagemLargura = 50;
  imagemMargin = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;

  // tslint:disable-next-line: variable-name
  _filtroLista: string;

  constructor(
      private eventoService: EventoService
    , private modalService: BsModalService
    ) { }

  openModal(tamplate: TemplateRef<any>) {
    this.modalRef = this.modalService.show(tamplate);
  }

  ngOnInit() {
    this.getEventos();
  }

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEvento(this.filtroLista) : this.eventos;
  }


  filtrarEvento(filtroPor: string): Evento[] {
    filtroPor = filtroPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtroPor) !== -1
    );
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  getEventos() {
    this.eventoService.getAllEvento().subscribe(
      // tslint:disable-next-line: variable-name
      (_eventos: Evento[]) => {
      this.eventos = _eventos;
      this.eventosFiltrados = this.eventos;
      console.log(_eventos);
    }, error => {
      console.log(error);
    });
  }
}
